using BookFast.SeedWork.Modeling;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BookFast.SeedWork.CommandStack
{
    public abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRepository repository;
        private readonly IMediator mediator;

        private bool eventsAvailable;

        protected CommandHandler(IRepository repository, IMediator mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var response = await HandleAsync(request, cancellationToken);

                transactionScope.Complete();

                if (eventsAvailable)
                {
                    await mediator.Publish(new EventsAvailableNotification());
                }

                return response; 
            }
        }

        protected abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);

        protected async Task CommitAsync(IEntity entity)
        {
            var events = entity.CollectEvents().ToList();

            var integrationEvents = events.OfType<IntegrationEvent>().ToList();
            if (integrationEvents.Any())
            {
                await repository.PersistEventsAsync(integrationEvents);
                eventsAvailable = true;
            }

            await repository.SaveChangesAsync();

            foreach (var @event in events.Except(integrationEvents).OrderBy(evt => evt.OccurredAt))
            {
                await mediator.Publish(@event);
            }
        }
    }

    public abstract class CommandHandler<TRequest> : CommandHandler<TRequest, Unit> where TRequest : IRequest
    {
        protected CommandHandler(IRepository repository, IMediator mediator) : base(repository, mediator)
        {
        }

        protected override async Task<Unit> HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            await HandleRequestAsync(request, cancellationToken);
            return Unit.Value;
        }

        protected abstract Task HandleRequestAsync(TRequest request, CancellationToken cancellationToken);
    }
}

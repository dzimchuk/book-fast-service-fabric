using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BookFast.SeedWork.CommandStack
{
    public abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly CommandContext context;

        protected CommandHandler(CommandContext context)
        {
            this.context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var response = await HandleAsync(request, cancellationToken);

                transactionScope.Complete();

                if (context.EventsAvailable)
                {
                    await context.Mediator.Publish(new EventsAvailableNotification());
                }

                return response;
            }
        }

        protected abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class CommandHandler<TRequest> : CommandHandler<TRequest, Unit> where TRequest : IRequest
    {
        protected CommandHandler(CommandContext context) : base(context)
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

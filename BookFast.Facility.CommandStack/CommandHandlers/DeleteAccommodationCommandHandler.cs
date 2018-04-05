using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class DeleteAccommodationCommandHandler : IRequestHandler<DeleteAccommodationCommand>
    {
        private readonly IAccommodationRepository repository;
        private readonly IMediator mediator;

        public DeleteAccommodationCommandHandler(IAccommodationRepository repository, IMediator mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        public async Task Handle(DeleteAccommodationCommand message, CancellationToken cancellationToken)
        {
            var accommodation = await repository.FindAsync(message.AccommodationId);
            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(message.AccommodationId);
            }

            accommodation.Delete();
            await repository.DeleteAsync(accommodation.Id);

            await accommodation.RaiseEventsAsync(mediator);
        }
    }
}

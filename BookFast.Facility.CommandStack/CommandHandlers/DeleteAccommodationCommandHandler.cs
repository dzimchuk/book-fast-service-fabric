using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using BookFast.ReliableEvents.CommandStack;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class DeleteAccommodationCommandHandler : AsyncRequestHandler<DeleteAccommodationCommand>
    {
        private readonly IAccommodationRepository repository;
        private readonly CommandContext context;

        public DeleteAccommodationCommandHandler(IAccommodationRepository repository, CommandContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override async Task Handle(DeleteAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodation = await repository.FindAsync(request.AccommodationId);
            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(request.AccommodationId);
            }

            accommodation.Delete();
            await repository.DeleteAsync(accommodation.Id);

            await repository.SaveChangesAsync(accommodation, context);
        }
    }
}

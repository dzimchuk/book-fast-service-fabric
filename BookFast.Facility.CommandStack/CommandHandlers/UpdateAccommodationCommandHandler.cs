using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using BookFast.ReliableEvents.CommandStack;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class UpdateAccommodationCommandHandler : AsyncRequestHandler<UpdateAccommodationCommand>
    {
        private readonly IAccommodationRepository repository;
        private readonly CommandContext context;

        public UpdateAccommodationCommandHandler(IAccommodationRepository repository, CommandContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override async Task Handle(UpdateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodation = await repository.FindAsync(request.AccommodationId);
            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(request.AccommodationId);
            }

            accommodation.Update(
                request.Name,
                request.Description,
                request.RoomCount,
                request.Images);

            await repository.UpdateAsync(accommodation);

            await repository.SaveChangesAsync(accommodation, context);
        }
    }
}

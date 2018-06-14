using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class UpdateAccommodationCommandHandler : IRequestHandler<UpdateAccommodationCommand>
    {
        private readonly IAccommodationRepository repository;

        public UpdateAccommodationCommandHandler(IAccommodationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(UpdateAccommodationCommand message, CancellationToken cancellationToken)
        {
            var accommodation = await repository.FindAsync(message.AccommodationId);
            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(message.AccommodationId);
            }

            accommodation.Update(
                message.Name,
                message.Description,
                message.RoomCount,
                message.Images);

            await repository.UpdateAsync(accommodation);

            await repository.PersistEventsAsync(accommodation);
            await repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

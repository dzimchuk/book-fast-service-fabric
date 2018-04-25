using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class CreateAccommodationCommandHandler : IRequestHandler<CreateAccommodationCommand, int>
    {
        private readonly IAccommodationRepository repository;

        public CreateAccommodationCommandHandler(IAccommodationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(CreateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodation = Domain.Models.Accommodation.NewAccommodation(
                request.FacilityId,
                request.Name,
                request.Description,
                request.RoomCount,
                request.Images);

            accommodation.Id = await repository.AddAsync(accommodation);

            await repository.PersistEventsAsync(accommodation);
            await repository.SaveChangesAsync();
            
            return accommodation.Id;
        }
    }
}

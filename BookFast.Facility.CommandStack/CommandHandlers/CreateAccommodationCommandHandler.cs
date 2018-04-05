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
        private readonly IMediator mediator;

        public CreateAccommodationCommandHandler(IAccommodationRepository repository, IMediator mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        public async Task<int> Handle(CreateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodation = Domain.Models.Accommodation.NewAccommodation(
                request.FacilityId,
                request.Name,
                request.Description,
                request.RoomCount,
                request.Images);

            accommodation.Id = await repository.CreateAsync(accommodation);

            await accommodation.RaiseEventsAsync(mediator);

            return accommodation.Id;
        }
    }
}

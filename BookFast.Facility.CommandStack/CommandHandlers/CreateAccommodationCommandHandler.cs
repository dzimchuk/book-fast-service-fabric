using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.SeedWork.CommandStack;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class CreateAccommodationCommandHandler : CommandHandler<CreateAccommodationCommand, int>
    {
        private readonly IAccommodationRepository repository;
        private readonly CommandContext context;

        public CreateAccommodationCommandHandler(IAccommodationRepository repository, CommandContext context)
            : base(context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override async Task<int> HandleAsync(CreateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodation = Domain.Models.Accommodation.NewAccommodation(
                request.FacilityId,
                request.Name,
                request.Description,
                request.RoomCount,
                request.Images);

            accommodation.Id = await repository.AddAsync(accommodation);

            await repository.SaveChangesAsync(accommodation, context);
            
            return accommodation.Id;
        }
    }
}

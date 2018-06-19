using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using BookFast.SeedWork.CommandStack;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class UpdateAccommodationCommandHandler : CommandHandler<UpdateAccommodationCommand>
    {
        private readonly IAccommodationRepository repository;
        private readonly CommandContext context;

        public UpdateAccommodationCommandHandler(IAccommodationRepository repository, CommandContext context)
            : base(context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override async Task HandleRequestAsync(UpdateAccommodationCommand message, CancellationToken cancellationToken)
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

            await repository.SaveChangesAsync(accommodation, context);
        }
    }
}

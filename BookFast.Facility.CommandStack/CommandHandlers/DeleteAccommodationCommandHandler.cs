using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using BookFast.SeedWork.CommandStack;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class DeleteAccommodationCommandHandler : CommandHandler<DeleteAccommodationCommand>
    {
        private readonly IAccommodationRepository repository;
        private readonly CommandContext context;

        public DeleteAccommodationCommandHandler(IAccommodationRepository repository, CommandContext context)
            : base(context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override async Task HandleRequestAsync(DeleteAccommodationCommand message, CancellationToken cancellationToken)
        {
            var accommodation = await repository.FindAsync(message.AccommodationId);
            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(message.AccommodationId);
            }

            accommodation.Delete();
            await repository.DeleteAsync(accommodation.Id);

            await repository.SaveChangesAsync(accommodation, context);
        }
    }
}

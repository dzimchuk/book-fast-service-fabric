using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using BookFast.SeedWork.CommandStack;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class DeleteFacilityCommandHandler : CommandHandler<DeleteFacilityCommand>
    {
        private readonly IFacilityRepository repository;
        private readonly CommandContext context;

        public DeleteFacilityCommandHandler(IFacilityRepository repository, CommandContext context)
            : base(context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override async Task HandleRequestAsync(DeleteFacilityCommand message, CancellationToken cancellationToken)
        {
            var facility = await repository.FindAsync(message.FacilityId);
            if (facility == null)
            {
                throw new FacilityNotFoundException(message.FacilityId);
            }

            facility.Delete();
            await repository.DeleteAsync(facility.Id);

            await repository.SaveChangesAsync(facility, context);
        }
    }
}

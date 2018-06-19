using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using BookFast.SeedWork.CommandStack;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class UpdateFacilityCommandHandler : CommandHandler<UpdateFacilityCommand>
    {
        private readonly IFacilityRepository repository;
        private readonly CommandContext context;

        public UpdateFacilityCommandHandler(IFacilityRepository repository, CommandContext context)
            : base(context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override async Task HandleRequestAsync(UpdateFacilityCommand message, CancellationToken cancellationToken)
        {
            var facility = await repository.FindAsync(message.FacilityId);
            if (facility == null)
            {
                throw new FacilityNotFoundException(message.FacilityId);
            }

            facility.Update(
                message.Name,
                message.Description,
                message.StreetAddress,
                message.Latitude,
                message.Longitude,
                message.Images);

            await repository.UpdateAsync(facility);

            await repository.SaveChangesAsync(facility, context);
        }
    }
}

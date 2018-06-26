using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using BookFast.ReliableEvents.CommandStack;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class UpdateFacilityCommandHandler : AsyncRequestHandler<UpdateFacilityCommand>
    {
        private readonly IFacilityRepository repository;
        private readonly CommandContext context;

        public UpdateFacilityCommandHandler(IFacilityRepository repository, CommandContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override async Task Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
        {
            var facility = await repository.FindAsync(request.FacilityId);
            if (facility == null)
            {
                throw new FacilityNotFoundException(request.FacilityId);
            }

            facility.Update(
                request.Name,
                request.Description,
                request.StreetAddress,
                request.Latitude,
                request.Longitude,
                request.Images);

            await repository.UpdateAsync(facility);

            await repository.SaveChangesAsync(facility, context);
        }
    }
}

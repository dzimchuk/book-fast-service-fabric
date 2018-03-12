using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand>
    {
        private readonly IFacilityRepository repository;
        private readonly IMediator mediator;

        public UpdateFacilityCommandHandler(IFacilityRepository repository, IMediator mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        public async Task Handle(UpdateFacilityCommand message, CancellationToken cancellationToken)
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

            await facility.RaiseEventsAsync(mediator);
        }
    }
}

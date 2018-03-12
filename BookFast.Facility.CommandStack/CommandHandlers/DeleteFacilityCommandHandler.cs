using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand>
    {
        private readonly IFacilityRepository repository;
        private readonly IMediator mediator;

        public DeleteFacilityCommandHandler(IFacilityRepository repository, IMediator mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        public async Task Handle(DeleteFacilityCommand message, CancellationToken cancellationToken)
        {
            var facility = await repository.FindAsync(message.FacilityId);
            if (facility == null)
            {
                throw new FacilityNotFoundException(message.FacilityId);
            }

            facility.Delete();
            await repository.DeleteAsync(facility);

            await facility.RaiseEventsAsync(mediator);
        }
    }
}

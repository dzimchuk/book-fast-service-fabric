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

        public DeleteFacilityCommandHandler(IFacilityRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(DeleteFacilityCommand message, CancellationToken cancellationToken)
        {
            var facility = await repository.FindAsync(message.FacilityId);
            if (facility == null)
            {
                throw new FacilityNotFoundException(message.FacilityId);
            }

            facility.Delete();
            await repository.DeleteAsync(facility.Id);

            await repository.PersistEventsAsync(facility);
            await repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

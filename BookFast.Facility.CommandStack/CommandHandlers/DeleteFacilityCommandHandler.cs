using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Exceptions;
using BookFast.ReliableEvents.CommandStack;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class DeleteFacilityCommandHandler : AsyncRequestHandler<DeleteFacilityCommand>
    {
        private readonly IFacilityRepository repository;
        private readonly CommandContext context;

        public DeleteFacilityCommandHandler(IFacilityRepository repository, CommandContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override async Task Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
        {
            var facility = await repository.FindAsync(request.FacilityId);
            if (facility == null)
            {
                throw new FacilityNotFoundException(request.FacilityId);
            }

            facility.Delete();
            await repository.DeleteAsync(facility.Id);

            await repository.SaveChangesAsync(facility, context);
        }
    }
}

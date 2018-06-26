using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
using BookFast.ReliableEvents.CommandStack;
using BookFast.Security;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.CommandHandlers
{
    public class CreateFacilityCommandHandler : IRequestHandler<CreateFacilityCommand, int>
    {
        private readonly IFacilityRepository repository;
        private readonly ISecurityContext securityContext;
        private readonly CommandContext context;

        public CreateFacilityCommandHandler(IFacilityRepository repository, ISecurityContext securityContext, CommandContext context)
        {
            this.repository = repository;
            this.securityContext = securityContext;
            this.context = context;
        }

        public async Task<int> Handle(CreateFacilityCommand request, CancellationToken cancellationToken)
        {
            var facility = Domain.Models.Facility.NewFacility(
                securityContext.GetCurrentTenant(),
                request.Name,
                request.Description,
                request.StreetAddress,
                request.Latitude,
                request.Longitude,
                request.Images);

            facility.Id = await repository.AddAsync(facility);

            await repository.SaveChangesAsync(facility, context);

            return facility.Id;
        }
    }
}

using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.CommandStack.Repositories;
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

        public CreateFacilityCommandHandler(IFacilityRepository repository, ISecurityContext securityContext)
        {
            this.repository = repository;
            this.securityContext = securityContext;
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

            await repository.PersistEventsAsync(facility);
            await repository.SaveChangesAsync();

            return facility.Id;
        }
    }
}

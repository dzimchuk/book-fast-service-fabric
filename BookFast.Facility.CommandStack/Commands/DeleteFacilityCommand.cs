using MediatR;

namespace BookFast.Facility.CommandStack.Commands
{
    public class DeleteFacilityCommand : IRequest
    {
        public int FacilityId { get; set; }
    }
}

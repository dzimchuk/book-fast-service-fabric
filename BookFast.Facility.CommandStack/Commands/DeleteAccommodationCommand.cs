using MediatR;

namespace BookFast.Facility.CommandStack.Commands
{
    public class DeleteAccommodationCommand : IRequest
    {
        public int AccommodationId { get; set; }
    }
}

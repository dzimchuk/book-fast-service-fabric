using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class FacilityDeletedEvent : Event
    {
        public int Id { get; set; }
    }
}

using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class AccommodationDeletedEvent : Event
    {
        public int Id { get; set; }
    }
}

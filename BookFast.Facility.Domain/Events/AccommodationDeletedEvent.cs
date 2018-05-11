using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class AccommodationDeletedEvent : IntegrationEvent
    {
        public int Id { get; set; }
    }
}

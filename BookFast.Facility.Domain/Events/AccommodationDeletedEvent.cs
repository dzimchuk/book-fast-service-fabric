using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class AccommodationDeletedEvent : Event
    {
        public int AccommodationId { get; }

        public AccommodationDeletedEvent(int accommodationId)
        {
            AccommodationId = accommodationId;
        }
    }
}

using BookFast.Facility.Domain.Models;
using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class AccommodationUpdatedEvent : Event
    {
        public Accommodation Accommodation { get; }

        public AccommodationUpdatedEvent(Accommodation accommodation)
        {
            Accommodation = accommodation;
        }
    }
}

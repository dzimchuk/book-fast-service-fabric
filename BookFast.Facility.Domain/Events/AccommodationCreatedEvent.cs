using BookFast.Facility.Domain.Models;
using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class AccommodationCreatedEvent : Event
    {
        public Accommodation Accommodation { get; }

        public AccommodationCreatedEvent(Accommodation accommodation)
        {
            Accommodation = accommodation;
        }
    }
}

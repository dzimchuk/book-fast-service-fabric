using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class FacilityUpdatedEvent : Event
    {
        public Models.Facility Facility { get; }

        public FacilityUpdatedEvent(Models.Facility facility)
        {
            Facility = facility;
        }
    }
}

using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class FacilityCreatedEvent : Event
    {
        public Models.Facility Facility { get; }

        public FacilityCreatedEvent(Models.Facility facility)
        {
            Facility = facility;
        }
    }
}

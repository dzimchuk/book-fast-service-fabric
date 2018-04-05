using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class FacilityDeletedEvent : Event
    {
        public int FacilityId { get; }

        public FacilityDeletedEvent(int facilityId)
        {
            FacilityId = facilityId;
        }
    }
}

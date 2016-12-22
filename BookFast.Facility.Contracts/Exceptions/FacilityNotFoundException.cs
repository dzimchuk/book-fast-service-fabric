using System;

namespace BookFast.Facility.Contracts.Exceptions
{
    public class FacilityNotFoundException : Exception
    {
        public Guid FacilityId { get; }

        public FacilityNotFoundException(Guid facilityId)
        {
            FacilityId = facilityId;
        }
    }
}
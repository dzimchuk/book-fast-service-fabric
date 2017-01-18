using System;

namespace BookFast.Files.Contracts.Exceptions
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
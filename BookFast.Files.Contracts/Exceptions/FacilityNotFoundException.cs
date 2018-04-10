using System;

namespace BookFast.Files.Contracts.Exceptions
{
    public class FacilityNotFoundException : Exception
    {
        public int FacilityId { get; }

        public FacilityNotFoundException(int facilityId)
        {
            FacilityId = facilityId;
        }
    }
}
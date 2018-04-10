using System;

namespace BookFast.Web.Contracts.Exceptions
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
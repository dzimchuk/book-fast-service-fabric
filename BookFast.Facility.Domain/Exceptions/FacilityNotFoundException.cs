using BookFast.SeedWork;
using System;

namespace BookFast.Facility.Domain.Exceptions
{
    public class FacilityNotFoundException : BusinessException
    {
        public int FacilityId { get; }

        public FacilityNotFoundException(int facilityId)
            : base(ErrorCodes.FacilityNotFound, $"Facility {facilityId} not found.")
        {
            FacilityId = facilityId;
        }
    }
}
using BookFast.SeedWork;
using System;

namespace BookFast.Facility.Domain.Exceptions
{
    public class FacilityNotFoundException : FormattedException
    {
        public Guid FacilityId { get; }

        public FacilityNotFoundException(Guid facilityId)
            : base(ErrorCodes.FacilityNotFound, $"Facility {facilityId} not found.")
        {
            FacilityId = facilityId;
        }
    }
}
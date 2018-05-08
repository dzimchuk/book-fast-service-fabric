using BookFast.SeedWork;

namespace BookFast.Facility.Domain.Exceptions
{
    internal class FacilityNotEmptyException : BusinessException
    {
        public int FacilityId { get; }
        public int AccommodationCount { get; }

        public FacilityNotEmptyException(int facilityId, int accommodationCount)
            : base(ErrorCodes.FacilityNotEmpty, $"Facility {facilityId} cannot be removed as it's associated with {accommodationCount} accommodations.")
        {
            FacilityId = facilityId;
            AccommodationCount = accommodationCount;
        }
    }
}

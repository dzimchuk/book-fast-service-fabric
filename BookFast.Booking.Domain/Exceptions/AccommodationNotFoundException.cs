using BookFast.SeedWork;

namespace BookFast.Booking.Domain.Exceptions
{
    public class AccommodationNotFoundException : BusinessException
    {
        public int AccommodationId { get; }

        public AccommodationNotFoundException(int accommodationId)
            : base(ErrorCodes.AccommodationNotFound, $"Accommodation {accommodationId} not found.")
        {
            AccommodationId = accommodationId;
        }
    }
}
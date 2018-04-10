using System;

namespace BookFast.Booking.Contracts.Exceptions
{
    public class AccommodationNotFoundException : Exception
    {
        public int AccommodationId { get; }

        public AccommodationNotFoundException(int accommodationId)
        {
            AccommodationId = accommodationId;
        }
    }
}
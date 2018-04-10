using System;

namespace BookFast.Files.Contracts.Exceptions
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
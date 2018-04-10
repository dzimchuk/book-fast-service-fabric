using System;

namespace BookFast.Web.Contracts.Exceptions
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
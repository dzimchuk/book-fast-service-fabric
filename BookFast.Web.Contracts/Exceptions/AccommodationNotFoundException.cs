using System;

namespace BookFast.Web.Contracts.Exceptions
{
    public class AccommodationNotFoundException : Exception
    {
        public Guid AccommodationId { get; }

        public AccommodationNotFoundException(Guid accommodationId)
        {
            AccommodationId = accommodationId;
        }
    }
}
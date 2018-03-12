using BookFast.SeedWork;
using System;

namespace BookFast.Facility.Domain.Exceptions
{
    public class AccommodationNotFoundException : FormattedException
    {
        public Guid AccommodationId { get; }

        public AccommodationNotFoundException(Guid accommodationId)
            : base(ErrorCodes.AccommodationNotFound, $"Accommodation {accommodationId} not found.")
        {
            AccommodationId = accommodationId;
        }
    }
}
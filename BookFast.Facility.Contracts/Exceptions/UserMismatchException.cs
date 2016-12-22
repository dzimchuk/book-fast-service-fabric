using System;

namespace BookFast.Facility.Contracts.Exceptions
{
    public class UserMismatchException : Exception
    {
        public Guid BookingId { get; }

        public UserMismatchException(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
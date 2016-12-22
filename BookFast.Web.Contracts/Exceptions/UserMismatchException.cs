using System;

namespace BookFast.Web.Contracts.Exceptions
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
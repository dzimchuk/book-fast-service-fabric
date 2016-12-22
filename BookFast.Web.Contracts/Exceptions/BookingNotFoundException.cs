using System;

namespace BookFast.Web.Contracts.Exceptions
{
    public class BookingNotFoundException : Exception
    {
        public Guid BookingId { get; }

        public BookingNotFoundException(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
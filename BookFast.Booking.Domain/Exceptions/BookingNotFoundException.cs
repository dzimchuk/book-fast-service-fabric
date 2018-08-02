using BookFast.SeedWork;
using System;

namespace BookFast.Booking.Domain.Exceptions
{
    public class BookingNotFoundException : BusinessException
    {
        public Guid BookingId { get; }

        public BookingNotFoundException(Guid bookingId)
            : base(ErrorCodes.BookingNotFound, $"Booking record ${bookingId} not found.")
        {
            BookingId = bookingId;
        }
    }
}
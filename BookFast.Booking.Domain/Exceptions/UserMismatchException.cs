using BookFast.SeedWork;
using System;

namespace BookFast.Booking.Domain.Exceptions
{
    public class UserMismatchException : BusinessException
    {
        public Guid BookingId { get; }

        public UserMismatchException(Guid bookingId)
            : base(ErrorCodes.UserMismatch, $"Error updating booking record ${bookingId} as it belongs to another user.")
        {
            BookingId = bookingId;
        }
    }
}
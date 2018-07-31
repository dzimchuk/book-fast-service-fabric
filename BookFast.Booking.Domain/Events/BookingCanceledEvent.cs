using BookFast.SeedWork.Modeling;
using System;

namespace BookFast.Booking.Domain.Events
{
    public class BookingCanceledEvent : Event
    {
        public Guid Id { get; set; }
    }
}

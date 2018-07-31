using BookFast.SeedWork.Modeling;
using System;

namespace BookFast.Booking.Domain.Events
{
    public class BookingCreatedEvent : Event
    {
        public Guid Id { get; set; }
        public int AccommodationId { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
    }
}

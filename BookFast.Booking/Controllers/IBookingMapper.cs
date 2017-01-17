using System.Collections.Generic;
using BookFast.Booking.Models.Representations;
using BookFast.Booking.Models;
using BookFast.Booking.Contracts.Models;

namespace BookFast.Booking.Controllers
{
    public interface IBookingMapper
    {
        BookingDetails MapFrom(BookingData data);
        IEnumerable<BookingRepresentation> MapFrom(IEnumerable<Contracts.Models.Booking> bookings);
        BookingRepresentation MapFrom(Contracts.Models.Booking booking);
    }
}
using System.Collections.Generic;
using BookFast.Web.Contracts.Models;
using BookFast.Booking.Client.Models;

namespace BookFast.Web.Proxy
{
    public interface IBookingMapper
    {
        BookAccommodationCommand MapFrom(BookingDetails details);
        Contracts.Models.Booking MapFrom(BookingRepresentation booking);
        List<Contracts.Models.Booking> MapFrom(IList<BookingRepresentation> bookings);
    }
}
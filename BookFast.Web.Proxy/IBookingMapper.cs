using System.Collections.Generic;
using BookFast.Web.Contracts.Models;
using BookFast.Web.Proxy.Models;

namespace BookFast.Web.Proxy
{
    public interface IBookingMapper
    {
        BookingData MapFrom(BookingDetails details);
        Booking MapFrom(BookingRepresentation booking);
        List<Booking> MapFrom(IList<BookingRepresentation> bookings);
    }
}
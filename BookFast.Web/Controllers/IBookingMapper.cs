using System.Collections.Generic;
using BookFast.Web.Contracts.Models;
using BookFast.Web.ViewModels;

namespace BookFast.Web.Controllers
{
    public interface IBookingMapper
    {
        BookingDetails MapFrom(CreateBookingViewModel viewModel);
        IEnumerable<BookingViewModel> MapFrom(IEnumerable<Contracts.Models.Booking> bookings);
        BookingViewModel MapFrom(Contracts.Models.Booking booking);
    }
}
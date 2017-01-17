using System.Threading.Tasks;
using BookFast.Booking.Data.Models;
using BookFast.Framework;

namespace BookFast.Booking.Data.Commands
{
    internal class CreateBookingCommand : ICommand<BookFastContext>
    {
        private readonly Contracts.Models.Booking booking;
        private readonly IBookingMapper mapper;

        public CreateBookingCommand(Contracts.Models.Booking booking, IBookingMapper mapper)
        {
            this.booking = booking;
            this.mapper = mapper;
        }

        public Task ApplyAsync(BookFastContext model)
        {
            model.Bookings.Add(mapper.MapFrom(booking));
            return model.SaveChangesAsync();
        }
    }
}
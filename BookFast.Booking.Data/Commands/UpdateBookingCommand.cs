using System.Threading.Tasks;
using BookFast.Booking.Data.Models;
using Microsoft.EntityFrameworkCore;
using BookFast.Framework;

namespace BookFast.Booking.Data.Commands
{
    internal class UpdateBookingCommand : ICommand<BookFastContext>
    {
        private readonly Contracts.Models.Booking booking;

        public UpdateBookingCommand(Contracts.Models.Booking booking)
        {
            this.booking = booking;
        }

        public async Task ApplyAsync(BookFastContext model)
        {
            var existingBooking = await model.Bookings.FirstOrDefaultAsync(b => b.Id == booking.Id);
            if (existingBooking != null)
            {
                existingBooking.CanceledOn = booking.CanceledOn;
                existingBooking.CheckedInOn = booking.CheckedInOn;

                await model.SaveChangesAsync();
            }
        }
    }
}
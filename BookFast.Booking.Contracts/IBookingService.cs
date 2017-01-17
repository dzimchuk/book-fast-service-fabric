using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Booking.Contracts.Models;

namespace BookFast.Booking.Contracts
{
    public interface IBookingService
    {
        Task<Models.Booking> BookAsync(Guid accommodationId, BookingDetails details);
        Task<List<Models.Booking>> ListPendingAsync();
        Task CancelAsync(Guid id);
        Task<Models.Booking> FindAsync(Guid id);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Models;

namespace BookFast.Web.Contracts
{
    public interface IBookingProxy
    {
        Task BookAsync(string userId, int accommodationId, BookingDetails details);
        Task<List<Booking>> ListPendingAsync(string userId);
        Task CancelAsync(string userId, Guid id);
        Task<Booking> FindAsync(string userId, Guid id);
    }
}
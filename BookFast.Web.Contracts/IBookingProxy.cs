using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Models;

namespace BookFast.Web.Contracts
{
    public interface IBookingProxy
    {
        Task BookAsync(int facilityId, int accommodationId, BookingDetails details);
        Task<List<Booking>> ListPendingAsync();
        Task CancelAsync(int facilityId, Guid id);
        Task<Booking> FindAsync(int facilityId, Guid id);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Models;

namespace BookFast.Web.Contracts
{
    public interface IBookingProxy
    {
        Task BookAsync(Guid facilityId, Guid accommodationId, BookingDetails details);
        Task<List<Booking>> ListPendingAsync();
        Task CancelAsync(Guid facilityId, Guid id);
        Task<Booking> FindAsync(Guid facilityId, Guid id);
    }
}
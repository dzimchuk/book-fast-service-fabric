using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Booking.Business.Data
{
    public interface IBookingDataSource
    {
        Task CreateAsync(Contracts.Models.Booking booking);
        Task<List<Contracts.Models.Booking>> ListPendingAsync(string user);
        Task<Contracts.Models.Booking> FindAsync(Guid id);
        Task UpdateAsync(Contracts.Models.Booking booking);
    }
}
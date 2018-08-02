using BookFast.Booking.Domain.Models;
using BookFast.ReliableEvents.CommandStack;
using System;
using System.Threading.Tasks;

namespace BookFast.Booking.CommandStack.Data
{
    public interface IBookingRepository : IRepositoryWithReliableEvents<BookingRecord>
    {
        Task<BookingRecord> FindAsync(Guid id);
        Task AddAsync(BookingRecord booking);
        Task UpdateAsync(BookingRecord booking);
    }
}
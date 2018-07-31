using BookFast.Booking.QueryStack.Representations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Booking.QueryStack
{
    public interface IBookingQueryDataSource
    {
        Task<BookingRepresentation> FindAsync(Guid id);
        Task<IEnumerable<BookingRepresentation>> ListPendingAsync(string user);
    }
}

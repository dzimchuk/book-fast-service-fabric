using BookFast.Booking.Data.Mappers;
using BookFast.Booking.Data.Models;
using BookFast.Booking.QueryStack;
using BookFast.Booking.QueryStack.Representations;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.Booking.Data
{
    internal class BookingQueryDataSource : IBookingQueryDataSource
    {
        private readonly IReliableStateManager stateManager;

        public BookingQueryDataSource(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task<BookingRepresentation> FindAsync(Guid id)
        {
            ConditionalValue<BookingRecord> result;

            var dictionary = await stateManager.GetAllBookingsAsync();
            using (var transaction = stateManager.CreateTransaction())
            {
                result = await dictionary.TryGetValueAsync(transaction, id);
                await transaction.CommitAsync();
            }

            return result.HasValue ? result.Value.ToRepresentation() : null;
        }

        public async Task<IEnumerable<BookingRepresentation>> ListPendingAsync(string user)
        {
            ConditionalValue<List<BookingRecord>> result;

            var dictionary = await stateManager.GetUserBookingsAsync();
            using (var transaction = stateManager.CreateTransaction())
            {
                result = await dictionary.TryGetValueAsync(transaction, user);
                await transaction.CommitAsync();
            }

            return result.HasValue
                ? result.Value.Where(booking => booking.CanceledOn == null && booking.CheckedInOn == null).Select(booking => booking.ToRepresentation()).ToList()
                : new List<BookingRepresentation>();
        }
    }
}

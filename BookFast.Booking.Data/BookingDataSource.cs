using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Booking.Business.Data;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System.Linq;

namespace BookFast.Booking.Data
{
    internal class BookingDataSource : IBookingDataSource
    {
        private readonly IReliableStateManager stateManager;

        public BookingDataSource(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task CreateAsync(Contracts.Models.Booking booking)
        {
            var bookingsDictionary = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Contracts.Models.Booking>>(Constants.BookingsDictionary);
            var userBookingsDictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, List<Contracts.Models.Booking>>>(Constants.UserBookingsDictionary);
            using (var transaction = stateManager.CreateTransaction())
            {
                await bookingsDictionary.AddAsync(transaction, booking.Id, booking);

                var userBookings = await userBookingsDictionary.GetOrAddAsync(transaction, booking.User, new List<Contracts.Models.Booking>());
                userBookings.Add(booking);

                await userBookingsDictionary.SetAsync(transaction, booking.User, userBookings);

                await transaction.CommitAsync();
            }
        }

        public async Task<List<Contracts.Models.Booking>> ListPendingAsync(string user)
        {
            ConditionalValue<List<Contracts.Models.Booking>> result;

            var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, List<Contracts.Models.Booking>>>(Constants.UserBookingsDictionary);
            using (var transaction = stateManager.CreateTransaction())
            {
                result = await dictionary.TryGetValueAsync(transaction, user);
                await transaction.CommitAsync();
            }

            return result.HasValue 
                ? result.Value.Where(booking => booking.CanceledOn == null && booking.CheckedInOn == null).ToList()
                : new List<Contracts.Models.Booking>();
        }

        public async Task<Contracts.Models.Booking> FindAsync(Guid id)
        {
            ConditionalValue<Contracts.Models.Booking> result;

            var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Contracts.Models.Booking>>(Constants.BookingsDictionary);
            using (var transaction = stateManager.CreateTransaction())
            {
                result = await dictionary.TryGetValueAsync(transaction, id);
                await transaction.CommitAsync();
            }

            return result.HasValue ? result.Value : null;
        }

        public async Task UpdateAsync(Contracts.Models.Booking booking)
        {
            var bookingsDictionary = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Contracts.Models.Booking>>(Constants.BookingsDictionary);
            var userBookingsDictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, List<Contracts.Models.Booking>>>(Constants.UserBookingsDictionary);
            using (var transaction = stateManager.CreateTransaction())
            {
                await bookingsDictionary.SetAsync(transaction, booking.Id, booking);

                var userBookings = (await userBookingsDictionary.TryGetValueAsync(transaction, booking.User, LockMode.Update)).Value;
                var existingBooking = userBookings.First(b => b.Id == booking.Id);
                userBookings.Remove(existingBooking);
                userBookings.Add(booking);

                await userBookingsDictionary.SetAsync(transaction, booking.User, userBookings);

                await transaction.CommitAsync();
            }
        }
    }
}
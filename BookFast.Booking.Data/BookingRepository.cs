using BookFast.Booking.CommandStack.Data;
using BookFast.Booking.Data.Mappers;
using BookFast.Booking.Domain.Models;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.Booking.Data
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly IReliableStateManager stateManager;

        public BookingRepository(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task AddAsync(BookingRecord booking)
        {
            var bookingsDictionary = await stateManager.GetAllBookingsAsync();
            var userBookingsDictionary = await stateManager.GetUserBookingsAsync();
            using (var transaction = stateManager.CreateTransaction())
            {
                var dataModel = booking.ToDataModel();
                await bookingsDictionary.AddAsync(transaction, booking.Id, dataModel);

                var userBookings = await userBookingsDictionary.GetOrAddAsync(transaction, booking.User, new List<Models.BookingRecord>());
                userBookings.Add(dataModel);

                await userBookingsDictionary.SetAsync(transaction, booking.User, userBookings);

                await transaction.CommitAsync();
            }
        }

        public async Task<BookingRecord> FindAsync(Guid id)
        {
            ConditionalValue<Models.BookingRecord> result;

            var dictionary = await stateManager.GetAllBookingsAsync();
            using (var transaction = stateManager.CreateTransaction())
            {
                result = await dictionary.TryGetValueAsync(transaction, id);
                await transaction.CommitAsync();
            }

            return result.HasValue ? result.Value.ToDomainModel() : null;
        }

        public async Task UpdateAsync(BookingRecord booking)
        {
            var bookingsDictionary = await stateManager.GetAllBookingsAsync();
            var userBookingsDictionary = await stateManager.GetUserBookingsAsync();
            using (var transaction = stateManager.CreateTransaction())
            {
                var dataModel = booking.ToDataModel();
                await bookingsDictionary.SetAsync(transaction, booking.Id, dataModel);

                var userBookings = (await userBookingsDictionary.TryGetValueAsync(transaction, booking.User, LockMode.Update)).Value;
                var existingBooking = userBookings.First(b => b.Id == booking.Id);
                userBookings.Remove(existingBooking);
                userBookings.Add(dataModel);

                await userBookingsDictionary.SetAsync(transaction, booking.User, userBookings);

                await transaction.CommitAsync();
            }
        }
    }
}

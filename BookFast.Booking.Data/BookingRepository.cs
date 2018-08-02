using BookFast.Booking.CommandStack.Data;
using BookFast.Booking.Data.Mappers;
using BookFast.Booking.Domain.Models;
using BookFast.ReliableEvents;
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

        private readonly List<Models.BookingRecord> addedRecords = new List<Models.BookingRecord>();
        private readonly List<Models.BookingRecord> updatedRecords = new List<Models.BookingRecord>();
        private readonly List<Models.ReliableEvent> addedEvents = new List<Models.ReliableEvent>();

        public BookingRepository(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public Task AddAsync(BookingRecord booking)
        {
            addedRecords.Add(booking.ToDataModel());
            return Task.CompletedTask;
        }

        public Task UpdateAsync(BookingRecord booking)
        {
            updatedRecords.Add(booking.ToDataModel());
            return Task.CompletedTask;
        }
        public Task PersistEventsAsync(IEnumerable<ReliableEvent> events)
        {
            addedEvents.AddRange(events.Select(@event => @event.ToDataModel()));
            return Task.CompletedTask;
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

        public async Task SaveChangesAsync()
        {
            var bookingsDictionary = await stateManager.GetAllBookingsAsync();
            var userBookingsDictionary = await stateManager.GetUserBookingsAsync();
            var eventsDictionary = addedEvents.Any() ? await stateManager.GetAllReliableEventsAsync() : null;

            using (var transaction = stateManager.CreateTransaction())
            {
                foreach (var addedRecord in addedRecords)
                {
                    await bookingsDictionary.AddAsync(transaction, addedRecord.Id, addedRecord);

                    var userBookings = await userBookingsDictionary.GetOrAddAsync(transaction, addedRecord.User, new List<Models.BookingRecord>());
                    userBookings.Add(addedRecord);

                    await userBookingsDictionary.SetAsync(transaction, addedRecord.User, userBookings);
                }

                foreach (var updatedRecord in updatedRecords)
                {
                    await bookingsDictionary.SetAsync(transaction, updatedRecord.Id, updatedRecord);

                    var userBookings = (await userBookingsDictionary.TryGetValueAsync(transaction, updatedRecord.User, LockMode.Update)).Value;
                    var existingBooking = userBookings.First(b => b.Id == updatedRecord.Id);
                    userBookings.Remove(existingBooking);
                    userBookings.Add(updatedRecord);

                    await userBookingsDictionary.SetAsync(transaction, updatedRecord.User, userBookings);
                }

                foreach (var addedEvent in addedEvents)
                {
                    await eventsDictionary.AddAsync(transaction, addedEvent.Id, addedEvent);
                }

                await transaction.CommitAsync();
            }
        }
    }
}

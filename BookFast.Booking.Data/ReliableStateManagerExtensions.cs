using BookFast.Booking.Data.Models;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Booking.Data
{
    internal static class ReliableStateManagerExtensions
    {
        public const string BookingsDictionary = "bookings";
        public const string UserBookingsDictionary = "user-bookings";

        public static Task<IReliableDictionary<Guid, BookingRecord>> GetAllBookingsAsync(this IReliableStateManager stateManager) =>
            stateManager.GetOrAddAsync<IReliableDictionary<Guid, BookingRecord>>(BookingsDictionary);

        public static Task<IReliableDictionary<string, List<BookingRecord>>> GetUserBookingsAsync(this IReliableStateManager stateManager) =>
            stateManager.GetOrAddAsync<IReliableDictionary<string, List<BookingRecord>>>(UserBookingsDictionary);
    }
}

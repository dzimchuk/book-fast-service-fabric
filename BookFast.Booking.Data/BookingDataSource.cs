using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Booking.Business.Data;
using BookFast.Booking.Data.Commands;
using BookFast.Booking.Data.Models;
using BookFast.Booking.Data.Queries;

namespace BookFast.Booking.Data
{
    internal class BookingDataSource : IBookingDataSource
    {
        private readonly BookFastContext context;
        private readonly IBookingMapper mapper;

        public BookingDataSource(BookFastContext context, IBookingMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public Task CreateAsync(Contracts.Models.Booking booking)
        {
            var command = new CreateBookingCommand(booking, mapper);
            return command.ApplyAsync(context);
        }

        public Task<List<Contracts.Models.Booking>> ListPendingAsync(string user)
        {
            var query = new ListPendingBookingsByUserQuery(user);
            return query.ExecuteAsync(context);
        }

        public Task<Contracts.Models.Booking> FindAsync(Guid id)
        {
            var query = new FindBookingQuery(id);
            return query.ExecuteAsync(context);
        }

        public Task UpdateAsync(Contracts.Models.Booking booking)
        {
            var command = new UpdateBookingCommand(booking);
            return command.ApplyAsync(context);
        }
    }
}
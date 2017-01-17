using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Booking.Contracts;
using BookFast.Booking.Business.Data;
using BookFast.Security;
using BookFast.Booking.Contracts.Exceptions;

namespace BookFast.Booking.Business
{
    internal class BookingService : IBookingService
    {
        private readonly IBookingDataSource dataSource;
        private readonly ISecurityContext securityContext;
        private readonly IAccommodationDataSource accommodationDataSource;

        public BookingService(IBookingDataSource dataSource, ISecurityContext securityContext, IAccommodationDataSource accommodationDataSource)
        {
            this.dataSource = dataSource;
            this.securityContext = securityContext;
            this.accommodationDataSource = accommodationDataSource;
        }

        public async Task<Contracts.Models.Booking> BookAsync(Guid accommodationId, Contracts.Models.BookingDetails details)
        {
            await accommodationDataSource.CheckAccommodationAsync(accommodationId);

            var booking = new Contracts.Models.Booking
                          {
                              Id = Guid.NewGuid(),
                              User = securityContext.GetCurrentUser(),
                              AccommodationId = accommodationId,
                              Details = details
                          };

            await dataSource.CreateAsync(booking);
            return booking;
        }

        public Task<List<Contracts.Models.Booking>> ListPendingAsync()
        {
            return dataSource.ListPendingAsync(securityContext.GetCurrentUser());
        }

        public async Task CancelAsync(Guid id)
        {
            var booking = await dataSource.FindAsync(id);
            if (booking == null)
                throw new BookingNotFoundException(id);

            if (!securityContext.GetCurrentUser().Equals(booking.User, StringComparison.OrdinalIgnoreCase))
                throw new UserMismatchException(id);

            booking.CanceledOn = DateTimeOffset.Now;
            await dataSource.UpdateAsync(booking);
        }

        public async Task<Contracts.Models.Booking> FindAsync(Guid id)
        {
            var booking = await dataSource.FindAsync(id);
            if (booking == null)
                throw new BookingNotFoundException(id);

            return booking;
        }
    }
}
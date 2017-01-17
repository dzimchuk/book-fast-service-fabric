using System;
using System.Linq;
using System.Threading.Tasks;
using BookFast.Booking.Contracts.Models;
using BookFast.Booking.Data.Models;
using Microsoft.EntityFrameworkCore;
using BookFast.Framework;

namespace BookFast.Booking.Data.Queries
{
    internal class FindBookingQuery : IQuery<BookFastContext, Contracts.Models.Booking>
    {
        private readonly Guid id;

        public FindBookingQuery(Guid id)
        {
            this.id = id;
        }

        public Task<Contracts.Models.Booking> ExecuteAsync(BookFastContext model)
        {
            return (from b in model.Bookings
                    where b.Id == id
                    select new Contracts.Models.Booking
                           {
                               Id = b.Id,
                               AccommodationId = b.AccommodationId,
                               User = b.User,
                               Details = new BookingDetails
                                         {
                                             FromDate = b.FromDate,
                                             ToDate = b.ToDate
                                         },
                               AccommodationName = b.AccommodationName,
                               FacilityId = b.FacilityId,
                               FacilityName = b.FacilityName,
                               StreetAddress = b.StreetAddress,
                               CanceledOn = b.CanceledOn,
                               CheckedInOn = b.CheckedInOn
                           }).FirstOrDefaultAsync();
        }
    }
}
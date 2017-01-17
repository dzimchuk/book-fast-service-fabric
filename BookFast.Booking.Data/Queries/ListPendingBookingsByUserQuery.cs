using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Booking.Data.Models;
using System.Linq;
using BookFast.Booking.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using BookFast.Framework;

namespace BookFast.Booking.Data.Queries
{
    internal class ListPendingBookingsByUserQuery : IQuery<BookFastContext, List<Contracts.Models.Booking>>
    {
        private readonly string user;

        public ListPendingBookingsByUserQuery(string user)
        {
            this.user = user;
        }

        public Task<List<Contracts.Models.Booking>> ExecuteAsync(BookFastContext model)
        {
            return (from b in model.Bookings
                    where b.User.Equals(user, StringComparison.OrdinalIgnoreCase) &&
                          b.CanceledOn == null && b.CheckedInOn == null
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
                    }).ToListAsync();
        }
    }
}
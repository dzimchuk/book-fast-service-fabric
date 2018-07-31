using BookFast.Booking.Domain.Events;
using BookFast.Booking.Domain.Exceptions;
using BookFast.Security;
using BookFast.SeedWork.Modeling;
using System;

namespace BookFast.Booking.Domain.Models
{
    public class BookingRecord : Entity<Guid>, IAggregateRoot
    {
        public string User { get; private set; }

        public int AccommodationId { get; private set; }
        public string AccommodationName { get; private set; }
        public int FacilityId { get; private set; }
        public string FacilityName { get; private set; }
        public string StreetAddress { get; private set; }

        public DateTimeOffset FromDate { get; private set; }
        public DateTimeOffset ToDate { get; private set; }

        public DateTimeOffset? CanceledOn { get; private set; }
        public DateTimeOffset? CheckedInOn { get; private set; }

        private BookingRecord()
        {
        }

        public static BookingRecord FromStorage(
            Guid id,
            string user,
            int accommodationId,
            string accommodationName,
            int facilityId,
            string facilityName,
            string streetAddress,
            DateTimeOffset fromDate,
            DateTimeOffset toDate,
            DateTimeOffset? canceledOn,
            DateTimeOffset? checkedInOn)
        {
            return new BookingRecord
            {
                Id = id,
                User = user,
                AccommodationId = accommodationId,
                AccommodationName = accommodationName,
                FacilityId = facilityId,
                FacilityName = facilityName,
                StreetAddress = streetAddress,
                FromDate = fromDate,
                ToDate = toDate,
                CanceledOn = canceledOn,
                CheckedInOn = checkedInOn
            };
        }

        public static BookingRecord NewBooking(
            Accommodation accommodation,
            Facility facility,
            DateTimeOffset fromDate,
            DateTimeOffset toDate,
            ISecurityContext securityContext)
        {
            var bookingRecord = new BookingRecord
            {
                Id = Guid.NewGuid(),
                User = securityContext.GetCurrentUser(),
                AccommodationId = accommodation.Id,
                AccommodationName = accommodation.Name,
                FacilityId = facility.Id,
                FacilityName = facility.Name,
                StreetAddress = facility.StreetAddress,
                FromDate = fromDate,
                ToDate = toDate
            };

            bookingRecord.AddEvent(new BookingCreatedEvent
            {
                Id = bookingRecord.Id,
                AccommodationId = bookingRecord.AccommodationId,
                FromDate = bookingRecord.FromDate,
                ToDate = bookingRecord.ToDate
            });

            return bookingRecord;
        }

        public void Cancel(ISecurityContext securityContext)
        {
            if (!securityContext.GetCurrentUser().Equals(User, StringComparison.OrdinalIgnoreCase))
            {
                throw new UserMismatchException(Id);
            }

            CanceledOn = DateTimeOffset.Now;

            AddEvent(new BookingCanceledEvent { Id = Id });
        }
    }
}

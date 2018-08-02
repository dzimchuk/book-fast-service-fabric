using BookFast.Booking.Data.Models;
using BookFast.Booking.QueryStack.Representations;

namespace BookFast.Booking.Data.Mappers
{
    internal static class BookingRecordMapper
    {
        public static BookingRepresentation ToRepresentation(this BookingRecord record) =>
            new BookingRepresentation
            {
                Id = record.Id,
                AccommodationId = record.AccommodationId,
                AccommodationName = record.AccommodationName,
                FacilityId = record.FacilityId,
                FacilityName = record.FacilityName,
                StreetAddress = record.StreetAddress,
                FromDate = record.FromDate,
                ToDate = record.ToDate
            };

        public static Domain.Models.BookingRecord ToDomainModel(this BookingRecord record) =>
            Domain.Models.BookingRecord.FromStorage(record.Id,
                record.User,
                record.AccommodationId,
                record.AccommodationName,
                record.FacilityId,
                record.FacilityName,
                record.StreetAddress,
                record.FromDate,
                record.ToDate,
                record.CanceledOn,
                record.CheckedInOn);

        public static BookingRecord ToDataModel(this Domain.Models.BookingRecord record) =>
            new BookingRecord
            {
                Id = record.Id,
                AccommodationId = record.AccommodationId,
                AccommodationName = record.AccommodationName,
                FacilityId = record.FacilityId,
                FacilityName = record.FacilityName,
                StreetAddress = record.StreetAddress,
                FromDate = record.FromDate,
                ToDate = record.ToDate,
                CanceledOn = record.CanceledOn,
                CheckedInOn = record.CheckedInOn,
                User = record.User
            };
    }
}

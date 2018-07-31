using System;

namespace BookFast.Booking.Data.Models
{
    internal class BookingRecord
    {
        public Guid Id { get; set; }
        public string User { get; set; }

        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string StreetAddress { get; set; }

        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }

        public DateTimeOffset? CanceledOn { get; set; }
        public DateTimeOffset? CheckedInOn { get; set; }
    }
}

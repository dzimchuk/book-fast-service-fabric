using System;

namespace BookFast.Booking.Data.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public Guid AccommodationId { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public DateTimeOffset? CanceledOn { get; set; }
        public DateTimeOffset? CheckedInOn { get; set; }

        public string AccommodationName { get; set; }
        public Guid FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string StreetAddress { get; set; }
    }
}
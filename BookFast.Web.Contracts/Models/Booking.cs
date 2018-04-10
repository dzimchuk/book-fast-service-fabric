using System;

namespace BookFast.Web.Contracts.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string StreetAddress { get; set; }
        public BookingDetails Details { get; set; }
        public DateTimeOffset? CanceledOn { get; set; }
        public DateTimeOffset? CheckedInOn { get; set; }
    }
}
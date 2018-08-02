using System;
using System.Runtime.Serialization;

namespace BookFast.Booking.Data.Models
{
    [DataContract]
    internal class BookingRecord
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public int AccommodationId { get; set; }
        [DataMember]
        public string AccommodationName { get; set; }
        [DataMember]
        public int FacilityId { get; set; }
        [DataMember]
        public string FacilityName { get; set; }
        [DataMember]
        public string StreetAddress { get; set; }

        [DataMember]
        public DateTimeOffset FromDate { get; set; }
        [DataMember]
        public DateTimeOffset ToDate { get; set; }

        [DataMember]
        public DateTimeOffset? CanceledOn { get; set; }
        [DataMember]
        public DateTimeOffset? CheckedInOn { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace BookFast.Booking.Data.Models
{
    [DataContract]
    internal class ReliableEvent
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string EventName { get; set; }
        [DataMember]
        public DateTimeOffset OccurredAt { get; set; }

        [DataMember]
        public string User { get; set; }
        [DataMember]
        public string Tenant { get; set; }

        [DataMember]
        public string Payload { get; set; }
    }
}

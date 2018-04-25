using System;

namespace BookFast.Facility.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTimeOffset OccurredAt { get; set; }
        public string Payload { get; set; }
    }
}

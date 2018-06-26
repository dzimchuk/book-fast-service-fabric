using System;

namespace BookFast.ReliableEvents
{
    public class ReliableEvent
    {
        public int Id { get; set; }

        public string EventName { get; set; }
        public DateTimeOffset OccurredAt { get; set; }

        public string User { get; set; }
        public string Tenant { get; set; }

        public string Payload { get; set; }
    }
}

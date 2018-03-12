using MediatR;
using System;

namespace BookFast.SeedWork.Modeling
{
    public class Event : INotification
    {
        public DateTimeOffset OccurredAt { get; }

        public Event()
        {
            OccurredAt = DateTimeOffset.UtcNow;
        }
    }
}

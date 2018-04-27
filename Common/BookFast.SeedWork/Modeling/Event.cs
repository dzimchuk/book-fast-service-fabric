using MediatR;
using System;

namespace BookFast.SeedWork.Modeling
{
    public abstract class Event : INotification
    {
        public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
    }
}

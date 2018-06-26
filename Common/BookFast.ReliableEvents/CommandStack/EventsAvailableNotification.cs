using MediatR;
using System;

namespace BookFast.ReliableEvents.CommandStack
{
    public class EventsAvailableNotification : INotification
    {
        public Guid Id { get; }

        public EventsAvailableNotification()
        {
            Id = Guid.NewGuid();
        }
    }
}

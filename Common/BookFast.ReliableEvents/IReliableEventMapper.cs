using System;

namespace BookFast.ReliableEvents
{
    public interface IReliableEventMapper
    {
        Type GetEventType(string eventType);
    }
}

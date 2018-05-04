using BookFast.SeedWork;
using System;

namespace BookFast.ServiceBus
{
    public class EventMapperException : FormattedException
    {
        public EventMapperException(Exception innerException)
            : base("EventMapper", "Failed to map integration event.", innerException)
        {
        }
    }
}

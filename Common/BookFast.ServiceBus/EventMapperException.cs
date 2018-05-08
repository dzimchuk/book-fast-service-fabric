using BookFast.SeedWork;
using System;

namespace BookFast.ServiceBus
{
    public class EventMapperException : BusinessException
    {
        public EventMapperException(Exception innerException)
            : base("EventMapper", "Failed to map integration event.", innerException)
        {
        }
    }
}

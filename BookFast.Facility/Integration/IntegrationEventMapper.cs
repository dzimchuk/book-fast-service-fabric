using BookFast.ServiceBus;
using MediatR;
using Newtonsoft.Json.Linq;

namespace BookFast.Facility.Integration
{
    internal class IntegrationEventMapper : IEventMapper
    {
        public IBaseRequest MapEvent(string eventName, JObject payload)
        {
            return null;
        }
    }
}

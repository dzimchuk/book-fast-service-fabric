using BookFast.ServiceBus;
using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.Search.Indexer.Integration
{
    internal class IntegrationEventMapper : IEventMapper
    {
        public IBaseRequest MapEvent(string eventName, JObject payload)
        {
            throw new NotImplementedException();
        }
    }
}

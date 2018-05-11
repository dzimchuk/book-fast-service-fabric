using BookFast.Search.Indexer.Commands;
using BookFast.ServiceBus;
using MediatR;
using Newtonsoft.Json.Linq;

namespace BookFast.Search.Indexer.Integration
{
    internal class IntegrationEventMapper : IEventMapper
    {
        public IBaseRequest MapEvent(string eventName, JObject payload)
        {
            switch (eventName)
            {
                case "AccommodationCreatedEvent":
                case "AccommodationUpdatedEvent":
                    return MapUpdate(payload);
                case "AccommodationDeletedEvent":
                    return MapRemove(payload);
                default:
                    return null;
            }
        }

        private static UpdateAccommodationCommand MapUpdate(JObject payload)
        {
            return payload.ToObject<UpdateAccommodationCommand>();
        }

        private static RemoveAccommodationCommand MapRemove(JObject payload)
        {
            return new RemoveAccommodationCommand { Id = int.Parse(payload["Id"].ToString()) };
        }
    }
}

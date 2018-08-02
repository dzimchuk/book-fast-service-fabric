using BookFast.ReliableEvents;

namespace BookFast.Facility.Data.Mappers
{
    internal static class ReliableEventMapper
    {
        public static Models.ReliableEvent ToDataModel(this ReliableEvent @event) =>
            new Models.ReliableEvent
            {
                OccurredAt = @event.OccurredAt,
                EventName = @event.EventName,
                Payload = @event.Payload,
                Tenant = @event.Tenant,
                User = @event.User
            };

        public static ReliableEvent ToContractModel(this Models.ReliableEvent @event) =>
            new ReliableEvent
            {
                Id = @event.Id.ToString(),
                OccurredAt = @event.OccurredAt,
                EventName = @event.EventName,
                Payload = @event.Payload,
                Tenant = @event.Tenant,
                User = @event.User
            };
    }
}

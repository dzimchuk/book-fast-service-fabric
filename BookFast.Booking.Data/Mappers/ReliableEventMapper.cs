using BookFast.ReliableEvents;

namespace BookFast.Booking.Data.Mappers
{
    internal static class ReliableEventMapper
    {
        public static Models.ReliableEvent ToDataModel(this ReliableEvent @event) =>
            new Models.ReliableEvent
            {
                Id = @event.Id,
                OccurredAt = @event.OccurredAt,
                EventName = @event.EventName,
                Payload = @event.Payload,
                Tenant = @event.Tenant,
                User = @event.User
            };

        public static ReliableEvent ToContractModel(this Models.ReliableEvent @event) =>
            new ReliableEvent
            {
                Id = @event.Id,
                OccurredAt = @event.OccurredAt,
                EventName = @event.EventName,
                Payload = @event.Payload,
                Tenant = @event.Tenant,
                User = @event.User
            };
    }
}

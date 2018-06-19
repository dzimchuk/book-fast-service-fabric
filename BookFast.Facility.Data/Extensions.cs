using BookFast.Security;
using BookFast.SeedWork.Modeling;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Facility.Data
{
    internal static class Extensions
    {
        public static string ToJson(this string[] array)
        {
            return array != null ? JsonConvert.SerializeObject(array) : null;
        }

        public static string[] ToStringArray(this string json)
        {
            return !string.IsNullOrWhiteSpace(json) ? JsonConvert.DeserializeObject<string[]>(json) : null;
        }

        public static async Task PersistEventsAsync(this FacilityContext context, IEnumerable<Event> events, ISecurityContext securityContext)
        {
            foreach (var @event in events)
            {
                await context.Events.AddAsync(new Models.Event
                {
                    EventName = @event.GetType().Name,
                    OccurredAt = @event.OccurredAt,
                    User = securityContext.GetCurrentUser(),
                    Tenant = securityContext.GetCurrentTenant(),
                    Payload = JsonConvert.SerializeObject(@event)
                });
            }
        }
    }
}

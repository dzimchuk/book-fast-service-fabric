using BookFast.SeedWork.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookFast.ReliableEvents
{
    public class DefaultReliableEventMapper : IReliableEventMapper
    {
        private readonly Dictionary<string, Type> eventTypes;

        public DefaultReliableEventMapper(Assembly assembly)
        {
            eventTypes = assembly.GetExportedTypes()
                .Where(type => typeof(IntegrationEvent).IsAssignableFrom(type))
                .ToDictionary(type => type.Name, type => type);
        }

        public Type GetEventType(string eventType) => eventTypes.ContainsKey(eventType) ? eventTypes[eventType] : null;
    }
}

using MediatR;
using Newtonsoft.Json.Linq;

namespace BookFast.ServiceBus
{
    public interface IEventMapper
    {
        IBaseRequest MapEvent(string eventName, JObject payload);
    }
}

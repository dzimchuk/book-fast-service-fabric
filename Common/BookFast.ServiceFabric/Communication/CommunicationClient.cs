using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Fabric;

namespace BookFast.ServiceFabric.Communication
{
    public class CommunicationClient<T> : ICommunicationClient
    {
        public CommunicationClient(T api)
        {
            API = api;
        }

        public T API { get; }

        ResolvedServiceEndpoint ICommunicationClient.Endpoint { get; set; }
        string ICommunicationClient.ListenerName { get; set; }
        ResolvedServicePartition ICommunicationClient.ResolvedServicePartition { get; set; }
    }
}

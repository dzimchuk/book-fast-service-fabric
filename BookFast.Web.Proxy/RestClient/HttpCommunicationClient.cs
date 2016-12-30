using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Fabric;

namespace BookFast.Web.Proxy.RestClient
{
    internal class HttpCommunicationClient<T> : ICommunicationClient
    {
        public HttpCommunicationClient(T api)
        {
            API = api;
        }

        public T API { get; }

        ResolvedServiceEndpoint ICommunicationClient.Endpoint { get; set; }
        string ICommunicationClient.ListenerName { get; set; }
        ResolvedServicePartition ICommunicationClient.ResolvedServicePartition { get; set; }
    }
}

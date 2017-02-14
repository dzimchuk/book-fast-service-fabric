using Microsoft.ServiceFabric.Services.Communication.Client;
using System;
using System.Fabric;
using System.Threading.Tasks;

namespace BookFast.ServiceFabric.Communication
{
    public class CommunicationClient<T> : ICommunicationClient
    {
        private readonly Func<Task<T>> apiFactory;

        public CommunicationClient(Func<Task<T>> apiFactory)
        {
            this.apiFactory = apiFactory;
        }

        public Task<T> CreateApiClient() => apiFactory();

        ResolvedServiceEndpoint ICommunicationClient.Endpoint { get; set; }
        string ICommunicationClient.ListenerName { get; set; }
        ResolvedServicePartition ICommunicationClient.ResolvedServicePartition { get; set; }
    }
}

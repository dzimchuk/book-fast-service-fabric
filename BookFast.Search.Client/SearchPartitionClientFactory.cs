using BookFast.ServiceFabric.Communication;
using System;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.Extensions.Options;

namespace BookFast.Search.Client
{
    internal class SearchPartitionClientFactory : IPartitionClientFactory<CommunicationClient<IBookFastSearchAPI>>
    {
        private readonly ICommunicationClientFactory<CommunicationClient<IBookFastSearchAPI>> factory;
        private readonly ApiOptions apiOptions;

        public SearchPartitionClientFactory(ICommunicationClientFactory<CommunicationClient<IBookFastSearchAPI>> factory, IOptions<ApiOptions> apiOptions)
        {
            this.factory = factory;
            this.apiOptions = apiOptions.Value;
        }

        public ServicePartitionClient<CommunicationClient<IBookFastSearchAPI>> CreatePartitionClient() => 
            new ServicePartitionClient<CommunicationClient<IBookFastSearchAPI>>(factory, new Uri(apiOptions.ServiceUri));
    }
}

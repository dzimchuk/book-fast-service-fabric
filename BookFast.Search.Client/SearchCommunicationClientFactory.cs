using Microsoft.ServiceFabric.Services.Communication.Client;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using BookFast.ServiceFabric.Communication;
using BookFast.Rest;

namespace BookFast.Search.Client
{
    internal class SearchCommunicationClientFactory : CommunicationClientFactoryBase<CommunicationClient<IBookFastSearchAPI>>
    {
        private readonly IApiClientFactory<IBookFastSearchAPI> apiClientFactory;

        public SearchCommunicationClientFactory(IServicePartitionResolver resolver, IApiClientFactory<IBookFastSearchAPI> apiClientFactory)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            this.apiClientFactory = apiClientFactory;
        }

        protected override void AbortClient(CommunicationClient<IBookFastSearchAPI> client)
        {
        }

        protected override Task<CommunicationClient<IBookFastSearchAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CommunicationClient<IBookFastSearchAPI>(() => apiClientFactory.CreateApiClientAsync(new Uri(endpoint))));
        }

        protected override bool ValidateClient(CommunicationClient<IBookFastSearchAPI> client)
        {
            return true;
        }

        protected override bool ValidateClient(string endpoint, CommunicationClient<IBookFastSearchAPI> client)
        {
            return true;
        }
    }
}

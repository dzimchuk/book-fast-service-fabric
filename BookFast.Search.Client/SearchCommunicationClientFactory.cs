using Microsoft.ServiceFabric.Services.Communication.Client;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using BookFast.ServiceFabric.Communication;

namespace BookFast.Search.Client
{
    internal class SearchCommunicationClientFactory : CommunicationClientFactoryBase<CommunicationClient<IBookFastSearchAPI>>
    {
        public SearchCommunicationClientFactory(IServicePartitionResolver resolver)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
        }

        protected override void AbortClient(CommunicationClient<IBookFastSearchAPI> client)
        {
        }

        protected override Task<CommunicationClient<IBookFastSearchAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CommunicationClient<IBookFastSearchAPI>(new BookFastSearchAPI(new Uri(endpoint))));
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

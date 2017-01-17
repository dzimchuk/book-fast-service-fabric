using Microsoft.ServiceFabric.Services.Communication.Client;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;

namespace BookFast.Web.Proxy.RestClient
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

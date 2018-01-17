using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using System;
using BookFast.ServiceFabric.Communication;
using BookFast.Rest;

namespace BookFast.Facility.Client
{
    internal class FacilityCommunicationClientFactory : CommunicationClientFactoryBase<CommunicationClient<IBookFastFacilityAPI>>
    {
        private readonly IApiClientFactory<IBookFastFacilityAPI> apiClientFactory;

        public FacilityCommunicationClientFactory(IServicePartitionResolver resolver, IApiClientFactory<IBookFastFacilityAPI> apiClientFactory)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            this.apiClientFactory = apiClientFactory;
        }

        protected override void AbortClient(CommunicationClient<IBookFastFacilityAPI> client)
        {
            // client with persistent connections should abort their connections here.
            // HTTP clients don't hold persistent connections, so no action is taken.
        }

        protected override Task<CommunicationClient<IBookFastFacilityAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            // clients that maintain persistent connections to a service should 
            // create that connection here.
            // an HTTP client doesn't maintain a persistent connection.

            var client = new CommunicationClient<IBookFastFacilityAPI>(() =>
            {
                return apiClientFactory.CreateApiClientAsync(new Uri(endpoint));
            });

            return Task.FromResult(client);
        }

        protected override bool ValidateClient(CommunicationClient<IBookFastFacilityAPI> client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }

        protected override bool ValidateClient(string endpoint, CommunicationClient<IBookFastFacilityAPI> client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }
    }
}

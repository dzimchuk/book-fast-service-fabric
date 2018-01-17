using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using System;
using BookFast.ServiceFabric.Communication;
using BookFast.Rest;

namespace BookFast.Files.Client
{
    internal class FilesCommunicationClientFactory : CommunicationClientFactoryBase<CommunicationClient<IBookFastFilesAPI>>
    {
        private readonly IApiClientFactory<IBookFastFilesAPI> apiClientFactory;

        public FilesCommunicationClientFactory(IServicePartitionResolver resolver, IApiClientFactory<IBookFastFilesAPI> apiClientFactory)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            this.apiClientFactory = apiClientFactory;
        }

        protected override void AbortClient(CommunicationClient<IBookFastFilesAPI> client)
        {
            // client with persistent connections should abort their connections here.
            // HTTP clients don't hold persistent connections, so no action is taken.
        }

        protected override Task<CommunicationClient<IBookFastFilesAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            // clients that maintain persistent connections to a service should 
            // create that connection here.
            // an HTTP client doesn't maintain a persistent connection.

            var client = new CommunicationClient<IBookFastFilesAPI>(() =>
            {
                return apiClientFactory.CreateApiClientAsync(new Uri(endpoint));
            });

            return Task.FromResult(client);
        }

        protected override bool ValidateClient(CommunicationClient<IBookFastFilesAPI> client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }

        protected override bool ValidateClient(string endpoint, CommunicationClient<IBookFastFilesAPI> client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }
    }
}

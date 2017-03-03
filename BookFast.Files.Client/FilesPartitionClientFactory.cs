using BookFast.ServiceFabric.Communication;
using System;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.Extensions.Options;
using Microsoft.ServiceFabric.Services.Client;

namespace BookFast.Files.Client
{
    internal class FilesPartitionClientFactory : IPartitionClientFactory<CommunicationClient<IBookFastFilesAPI>>
    {
        private readonly ICommunicationClientFactory<CommunicationClient<IBookFastFilesAPI>> factory;
        private readonly ApiOptions apiOptions;

        public FilesPartitionClientFactory(ICommunicationClientFactory<CommunicationClient<IBookFastFilesAPI>> factory, IOptions<ApiOptions> apiOptions)
        {
            this.factory = factory;
            this.apiOptions = apiOptions.Value;
        }

        public ServicePartitionClient<CommunicationClient<IBookFastFilesAPI>> CreatePartitionClient() => 
            new ServicePartitionClient<CommunicationClient<IBookFastFilesAPI>>(factory, new Uri(apiOptions.ServiceUri));

        public ServicePartitionClient<CommunicationClient<IBookFastFilesAPI>> CreatePartitionClient(ServicePartitionKey partitionKey) => 
            new ServicePartitionClient<CommunicationClient<IBookFastFilesAPI>>(factory, new Uri(apiOptions.ServiceUri), partitionKey);
    }
}

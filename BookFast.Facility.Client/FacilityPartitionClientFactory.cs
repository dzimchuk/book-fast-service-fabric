using BookFast.ServiceFabric.Communication;
using System;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.Extensions.Options;
using Microsoft.ServiceFabric.Services.Client;

namespace BookFast.Facility.Client
{
    internal class FacilityPartitionClientFactory : IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>>
    {
        private readonly ICommunicationClientFactory<CommunicationClient<IBookFastFacilityAPI>> factory;
        private readonly ApiOptions apiOptions;

        public FacilityPartitionClientFactory(ICommunicationClientFactory<CommunicationClient<IBookFastFacilityAPI>> factory, IOptions<ApiOptions> apiOptions)
        {
            this.factory = factory;
            this.apiOptions = apiOptions.Value;
        }

        public ServicePartitionClient<CommunicationClient<IBookFastFacilityAPI>> CreatePartitionClient() => 
            new ServicePartitionClient<CommunicationClient<IBookFastFacilityAPI>>(factory, new Uri(apiOptions.ServiceUri));

        public ServicePartitionClient<CommunicationClient<IBookFastFacilityAPI>> CreatePartitionClient(ServicePartitionKey partitionKey) => 
            new ServicePartitionClient<CommunicationClient<IBookFastFacilityAPI>>(factory, new Uri(apiOptions.ServiceUri), partitionKey);
    }
}

using BookFast.ServiceFabric.Communication;
using System;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.Extensions.Options;

namespace BookFast.Facility.Client
{
    internal class FacilityPartitionClientFactory : IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>>
    {
        private readonly ICommunicationClientFactory<CommunicationClient<IBookFastFacilityAPI>> factory;
        private readonly Options options;

        public FacilityPartitionClientFactory(ICommunicationClientFactory<CommunicationClient<IBookFastFacilityAPI>> factory, IOptions<Options> options)
        {
            this.factory = factory;
            this.options = options.Value;
        }

        public ServicePartitionClient<CommunicationClient<IBookFastFacilityAPI>> CreatePartitionClient() => 
            new ServicePartitionClient<CommunicationClient<IBookFastFacilityAPI>>(factory, new Uri(options.ServiceUri));
    }
}

using BookFast.ServiceFabric.Communication;
using System;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.Extensions.Options;
using Microsoft.ServiceFabric.Services.Client;

namespace BookFast.Booking.Client
{
    internal class BookingPartitionClientFactory : IPartitionClientFactory<CommunicationClient<IBookFastBookingAPI>>
    {
        private readonly ICommunicationClientFactory<CommunicationClient<IBookFastBookingAPI>> factory;
        private readonly ApiOptions apiOptions;

        public BookingPartitionClientFactory(ICommunicationClientFactory<CommunicationClient<IBookFastBookingAPI>> factory, IOptions<ApiOptions> apiOptions)
        {
            this.factory = factory;
            this.apiOptions = apiOptions.Value;
        }

        public ServicePartitionClient<CommunicationClient<IBookFastBookingAPI>> CreatePartitionClient() => 
            new ServicePartitionClient<CommunicationClient<IBookFastBookingAPI>>(factory, new Uri(apiOptions.ServiceUri));

        public ServicePartitionClient<CommunicationClient<IBookFastBookingAPI>> CreatePartitionClient(ServicePartitionKey partitionKey) => 
            new ServicePartitionClient<CommunicationClient<IBookFastBookingAPI>>(factory, new Uri(apiOptions.ServiceUri), partitionKey);
    }
}

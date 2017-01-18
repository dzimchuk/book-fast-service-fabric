using BookFast.ServiceFabric.Communication;
using System;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.Extensions.Options;

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
    }
}

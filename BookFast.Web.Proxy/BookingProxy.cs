using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Models;
using BookFast.ServiceFabric.Communication;
using BookFast.Booking.Client;
using Microsoft.ServiceFabric.Services.Client;

namespace BookFast.Web.Proxy
{
    internal class BookingProxy : IBookingProxy
    {
        private readonly IPartitionClientFactory<CommunicationClient<IBookFastBookingAPI>> partitionClientFactory;
        private readonly IBookingMapper mapper;

        public BookingProxy(IPartitionClientFactory<CommunicationClient<IBookFastBookingAPI>> partitionClientFactory, IBookingMapper mapper)
        {
            this.partitionClientFactory = partitionClientFactory;
            this.mapper = mapper;
        }

        public async Task BookAsync(string userId, int accommodationId, BookingDetails details)
        {
            var data = mapper.MapFrom(details);
            data.AccommodationId = accommodationId;

            var result = await partitionClientFactory.CreatePartitionClient(new ServicePartitionKey(userId.ToPartitionKey())).InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.CreateBookingWithHttpMessagesAsync(accommodationId, data);
            });
            
            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }
        }

        public async Task<List<Contracts.Models.Booking>> ListPendingAsync(string userId)
        {
            var result = await partitionClientFactory.CreatePartitionClient(new ServicePartitionKey(userId.ToPartitionKey())).InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.ListBookingsWithHttpMessagesAsync();
            });

            return mapper.MapFrom(result.Body);
        }

        public async Task CancelAsync(string userId, Guid id)
        {
            var result = await partitionClientFactory.CreatePartitionClient(new ServicePartitionKey(userId.ToPartitionKey())).InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.DeleteBookingWithHttpMessagesAsync(id);
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new BookingNotFoundException(id);
            }
        }

        public async Task<Contracts.Models.Booking> FindAsync(string userId, Guid id)
        {
            var result = await partitionClientFactory.CreatePartitionClient(new ServicePartitionKey(userId.ToPartitionKey())).InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.FindBookingWithHttpMessagesAsync(id);
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new BookingNotFoundException(id);
            }

            return mapper.MapFrom(result.Body);
        }
    }
}
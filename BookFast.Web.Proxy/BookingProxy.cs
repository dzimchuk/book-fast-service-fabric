using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Models;
using BookFast.Booking.Client;
using BookFast.Framework;
using BookFast.Rest;

namespace BookFast.Web.Proxy
{
    internal class BookingProxy : IBookingService
    {
        private readonly IBookingMapper mapper;
        private readonly IApiClientFactory<IBookFastBookingAPI> apiClientFactory;

        public BookingProxy(IBookingMapper mapper, IApiClientFactory<IBookFastBookingAPI> apiClientFactory)
        {
            this.mapper = mapper;
            this.apiClientFactory = apiClientFactory;
        }

        public async Task BookAsync(Guid facilityId, Guid accommodationId, BookingDetails details)
        {
            var data = mapper.MapFrom(details);
            data.AccommodationId = accommodationId;

            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.CreateBookingWithHttpMessagesAsync(accommodationId, data, facilityId.ToPartitionKey());

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }
        }

        public async Task<List<Contracts.Models.Booking>> ListPendingAsync()
        {
            var api = await apiClientFactory.CreateApiClientAsync();

            var queries = new List<Task<List<Contracts.Models.Booking>>>();
            for (var i = 0; i < 16; i++)
            {
                queries.Add(ListPartitionAsync(api, i));
            }

            await Task.WhenAll(queries);

            var result = new List<Contracts.Models.Booking>();
            foreach (var query in queries)
            {
                result.AddRange(await query);
            }

            return result;
        }

        private async Task<List<Contracts.Models.Booking>> ListPartitionAsync(IBookFastBookingAPI api, long partitionKey)
        {
            var result = await api.ListBookingsWithHttpMessagesAsync(partitionKey);
            return mapper.MapFrom(result.Body);
        }

        public async Task CancelAsync(Guid facilityId, Guid id)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.DeleteBookingWithHttpMessagesAsync(id, facilityId.ToPartitionKey());

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new BookingNotFoundException(id);
            }
        }

        public async Task<Contracts.Models.Booking> FindAsync(Guid facilityId, Guid id)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.FindBookingWithHttpMessagesAsync(id, facilityId.ToPartitionKey());

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new BookingNotFoundException(id);
            }

            return mapper.MapFrom(result.Body);
        }
    }
}
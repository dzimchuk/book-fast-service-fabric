using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Models;
using Microsoft.ServiceFabric.Services.Communication.Client;
using BookFast.Web.Proxy.RestClient;
using Microsoft.Extensions.Options;

namespace BookFast.Web.Proxy
{
    internal class AccommodationProxy : IAccommodationService
    {
        private readonly IAccommodationMapper mapper;
        private readonly ICommunicationClientFactory<FacilityClient> factory;
        private readonly ApiOptions apiOptions;

        public AccommodationProxy(IAccommodationMapper mapper, 
            ICommunicationClientFactory<FacilityClient> factory, 
            IOptions<ApiOptions> apiOptions)
        {
            this.mapper = mapper;
            this.factory = factory;
            this.apiOptions = apiOptions.Value;
        }

        private ServicePartitionClient<FacilityClient> PartitionClient =>
            new ServicePartitionClient<FacilityClient>(factory, new Uri(apiOptions.FacilityService));

        public async Task<List<Accommodation>> ListAsync(Guid facilityId)
        {
            var result = await PartitionClient.InvokeWithRetryAsync(async client =>
            {
                var api = await client.GetApiAsync();
                return await api.ListAccommodationsWithHttpMessagesAsync(facilityId);
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new FacilityNotFoundException(facilityId);

            return mapper.MapFrom(result.Body);
        }

        public async Task<Accommodation> FindAsync(Guid accommodationId)
        {
            var result = await PartitionClient.InvokeWithRetryAsync(async client =>
            {
                var api = await client.GetApiAsync();
                return await api.FindAccommodationWithHttpMessagesAsync(accommodationId);
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new AccommodationNotFoundException(accommodationId);

            return mapper.MapFrom(result.Body);
        }

        public async Task CreateAsync(Guid facilityId, AccommodationDetails details)
        {
            var result = await PartitionClient.InvokeWithRetryAsync(async client =>
            {
                var api = await client.GetApiAsync();
                return await api.CreateAccommodationWithHttpMessagesAsync(facilityId, mapper.MapFrom(details));
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new FacilityNotFoundException(facilityId);
        }

        public async Task UpdateAsync(Guid accommodationId, AccommodationDetails details)
        {
            var result = await PartitionClient.InvokeWithRetryAsync(async client =>
            {
                var api = await client.GetApiAsync();
                return await api.UpdateAccommodationWithHttpMessagesAsync(accommodationId, mapper.MapFrom(details));
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new AccommodationNotFoundException(accommodationId);
        }

        public async Task DeleteAsync(Guid accommodationId)
        {
            var result = await PartitionClient.InvokeWithRetryAsync(async client =>
            {
                var api = await client.GetApiAsync();
                return await api.DeleteAccommodationWithHttpMessagesAsync(accommodationId);
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new AccommodationNotFoundException(accommodationId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Models;
using BookFast.Facility.Client;
using BookFast.Rest;

namespace BookFast.Web.Proxy
{
    internal class AccommodationProxy : IAccommodationService
    {
        private readonly IAccommodationMapper mapper;
        private readonly IApiClientFactory<IBookFastFacilityAPI> apiClientFactory;

        public AccommodationProxy(IAccommodationMapper mapper,
            IApiClientFactory<IBookFastFacilityAPI> apiClientFactory)
        {
            this.mapper = mapper;
            this.apiClientFactory = apiClientFactory;
        }

        public async Task<List<Accommodation>> ListAsync(Guid facilityId)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.ListAccommodationsWithHttpMessagesAsync(facilityId);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }

            return mapper.MapFrom(result.Body);
        }

        public async Task<Accommodation> FindAsync(Guid accommodationId)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.FindAccommodationWithHttpMessagesAsync(accommodationId);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }

            return mapper.MapFrom(result.Body);
        }

        public async Task CreateAsync(Guid facilityId, AccommodationDetails details)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.CreateAccommodationWithHttpMessagesAsync(facilityId, mapper.MapFrom(details));

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }
        }

        public async Task UpdateAsync(Guid accommodationId, AccommodationDetails details)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.UpdateAccommodationWithHttpMessagesAsync(accommodationId, mapper.MapFrom(details));

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }
        }

        public async Task DeleteAsync(Guid accommodationId)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.DeleteAccommodationWithHttpMessagesAsync(accommodationId);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }
        }
    }
}
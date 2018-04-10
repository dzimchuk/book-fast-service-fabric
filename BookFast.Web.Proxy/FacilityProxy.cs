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
    internal class FacilityProxy : IFacilityProxy
    {
        private readonly IFacilityMapper mapper;
        private readonly IApiClientFactory<IBookFastFacilityAPI> apiClientFactory;

        public FacilityProxy(IFacilityMapper mapper,
            IApiClientFactory<IBookFastFacilityAPI> apiClientFactory)
        {
            this.mapper = mapper;
            this.apiClientFactory = apiClientFactory;
        }
                
        public async Task<List<Contracts.Models.Facility>> ListAsync()
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.ListFacilitiesWithHttpMessagesAsync();

            return mapper.MapFrom(result.Body);
        }

        public async Task<Contracts.Models.Facility> FindAsync(int facilityId)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.FindFacilityWithHttpMessagesAsync(facilityId);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }

            return mapper.MapFrom(result.Body);
        }

        public async Task CreateAsync(FacilityDetails details)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            await api.CreateFacilityWithHttpMessagesAsync(mapper.ToCreateCommand(details));
        }

        public async Task UpdateAsync(int facilityId, FacilityDetails details)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.UpdateFacilityWithHttpMessagesAsync(facilityId, mapper.ToUpdateCommand(details));

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }
        }

        public async Task DeleteAsync(int facilityId)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.DeleteFacilityWithHttpMessagesAsync(facilityId);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }
        }
    }
}
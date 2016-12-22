using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Files;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BookFast.Web.Proxy
{
    internal class FileAccessProxy : IFileAccessProxy
    {
        private readonly IBookFastAPIFactory restClientFactory;
        private readonly IFileAccessMapper mapper;

        public FileAccessProxy(IBookFastAPIFactory restClientFactory, IFileAccessMapper mapper)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
        }

        public async Task<FileAccessToken> IssueAccommodationImageUploadTokenAsync(Guid accommodationId, string originalFileName)
        {
            var client = await restClientFactory.CreateAsync();
            var result = await client.GetAccommodationImageUploadTokenWithHttpMessagesAsync(accommodationId, originalFileName);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new AccommodationNotFoundException(accommodationId);

            return mapper.MapFrom(result.Body);
        }

        public async Task<FileAccessToken> IssueFacilityImageUploadTokenAsync(Guid facilityId, string originalFileName)
        {
            var client = await restClientFactory.CreateAsync();
            var result = await client.GetFacilityImageUploadTokenWithHttpMessagesAsync(facilityId, originalFileName);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new FacilityNotFoundException(facilityId);

            return mapper.MapFrom(result.Body);
        }
    }
}

using BookFast.Files.Client;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Files;
using System;
using System.Net;
using System.Threading.Tasks;
using BookFast.Rest;

namespace BookFast.Web.Proxy
{
    internal class FileAccessProxy : IFileAccessProxy
    {
        private readonly IFileAccessMapper mapper;
        private readonly IApiClientFactory<IBookFastFilesAPI> apiClientFactory;

        public FileAccessProxy(IFileAccessMapper mapper, IApiClientFactory<IBookFastFilesAPI> apiClientFactory)
        {
            this.mapper = mapper;
            this.apiClientFactory = apiClientFactory;
        }

        public async Task<FileAccessToken> IssueAccommodationImageUploadTokenAsync(Guid accommodationId, string originalFileName)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.GetAccommodationImageUploadTokenWithHttpMessagesAsync(accommodationId, originalFileName);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }

            return mapper.MapFrom(result.Body);
        }

        public async Task<FileAccessToken> IssueFacilityImageUploadTokenAsync(Guid facilityId, string originalFileName)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.GetFacilityImageUploadTokenWithHttpMessagesAsync(facilityId, originalFileName);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }

            return mapper.MapFrom(result.Body);
        }
    }
}

using BookFast.Files.Client;
using BookFast.ServiceFabric.Communication;
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
        private readonly IFileAccessMapper mapper;
        private readonly IPartitionClientFactory<CommunicationClient<IBookFastFilesAPI>> partitionClientFactory;

        public FileAccessProxy(IFileAccessMapper mapper, IPartitionClientFactory<CommunicationClient<IBookFastFilesAPI>> partitionClientFactory)
        {
            this.mapper = mapper;
            this.partitionClientFactory = partitionClientFactory;
        }

        public async Task<FileAccessToken> IssueAccommodationImageUploadTokenAsync(Guid accommodationId, string originalFileName)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.GetAccommodationImageUploadTokenWithHttpMessagesAsync(accommodationId, originalFileName);
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }

            return mapper.MapFrom(result.Body);
        }

        public async Task<FileAccessToken> IssueFacilityImageUploadTokenAsync(Guid facilityId, string originalFileName)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.GetFacilityImageUploadTokenWithHttpMessagesAsync(facilityId, originalFileName);
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }

            return mapper.MapFrom(result.Body);
        }
    }
}

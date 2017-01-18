using BookFast.Files.Contracts;
using System;
using System.Threading.Tasks;
using BookFast.Files.Contracts.Models;
using System.IO;
using BookFast.Files.Business.Data;
using BookFast.Files.Contracts.Exceptions;

namespace BookFast.Files.Business
{
    internal class FileAccessService : IFileAccessService
    {
        private readonly IAccessTokenProvider tokenProvider;
        private readonly IFacilityProxy facilityService;

        private const double TokenExpirationTime = 20;

        public FileAccessService(IAccessTokenProvider tokenProvider, IFacilityProxy facilityService)
        {
            this.tokenProvider = tokenProvider;
            this.facilityService = facilityService;
        }

        public async Task<FileAccessToken> IssueAccommodationImageUploadTokenAsync(Guid accommodationId, string originalFileName)
        {
            var accommodation = await facilityService.FindAccommodationAsync(accommodationId);
            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }

            var fileName = GenerateName(originalFileName);
            var path = ConstructPath(accommodation.FacilityId, accommodation.Id, fileName);
            return IssueImageUploadToken(path);
        }

        public async Task<FileAccessToken> IssueFacilityImageUploadTokenAsync(Guid facilityId, string originalFileName)
        {
            var facility = await facilityService.FindFacilityAsync(facilityId);
            if (facility == null)
            {
                throw new FacilityNotFoundException(facilityId);
            }

            var fileName = GenerateName(originalFileName);
            var path = ConstructPath(facilityId, fileName);
            return IssueImageUploadToken(path);
        }

        private FileAccessToken IssueImageUploadToken(string path)
        {
            var url = tokenProvider.GetUrlWithAccessToken(path, AccessPermission.Write, DateTimeOffset.Now.AddMinutes(TokenExpirationTime));
            return new FileAccessToken
            {
                AccessPermission = AccessPermission.Write,
                Url = url
            };
        }

        private string GenerateName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".jpg";
            }

            return $"{Path.GetRandomFileName()}{extension}";
        }

        private string ConstructPath(Guid facilityId, Guid accommodationId, string fileName) => $"{facilityId}/{accommodationId}/{fileName}";

        private string ConstructPath(Guid facilityId, string fileName) => $"{facilityId}/{fileName}";
    }
}

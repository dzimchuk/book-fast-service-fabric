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
        private readonly ISASTokenProvider tokenProvider;
        private readonly IFacilityProxy facilityProxy;

        private const double TokenExpirationTime = 20;

        public FileAccessService(ISASTokenProvider tokenProvider, IFacilityProxy facilityProxy)
        {
            this.tokenProvider = tokenProvider;
            this.facilityProxy = facilityProxy;
        }

        public async Task<FileAccessToken> IssueAccommodationImageUploadTokenAsync(int accommodationId, string originalFileName)
        {
            var accommodation = await facilityProxy.FindAccommodationAsync(accommodationId);
            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }

            var fileName = GenerateName(originalFileName);
            var path = ConstructPath(accommodation.FacilityId, accommodation.Id, fileName);
            return IssueImageUploadToken(path);
        }

        public async Task<FileAccessToken> IssueFacilityImageUploadTokenAsync(int facilityId, string originalFileName)
        {
            var facility = await facilityProxy.FindFacilityAsync(facilityId);
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

        private string ConstructPath(int facilityId, int accommodationId, string fileName) => $"{facilityId}/{accommodationId}/{fileName}";

        private string ConstructPath(int facilityId, string fileName) => $"{facilityId}/{fileName}";
    }
}

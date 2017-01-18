using BookFast.Files.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace BookFast.Files.Contracts
{
    public interface IFileAccessService
    {
        Task<FileAccessToken> IssueFacilityImageUploadTokenAsync(Guid facilityId, string originalFileName);
        Task<FileAccessToken> IssueAccommodationImageUploadTokenAsync(Guid accommodationId, string originalFileName);
    }
}

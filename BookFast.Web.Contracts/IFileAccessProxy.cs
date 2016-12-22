using System;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Files;

namespace BookFast.Web.Contracts
{
    public interface IFileAccessProxy
    {
        Task<FileAccessToken> IssueAccommodationImageUploadTokenAsync(Guid accommodationId, string originalFileName);
        Task<FileAccessToken> IssueFacilityImageUploadTokenAsync(Guid facilityId, string originalFileName);
    }
}
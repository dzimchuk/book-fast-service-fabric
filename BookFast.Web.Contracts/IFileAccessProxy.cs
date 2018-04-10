using System;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Files;

namespace BookFast.Web.Contracts
{
    public interface IFileAccessProxy
    {
        Task<FileAccessToken> IssueAccommodationImageUploadTokenAsync(int accommodationId, string originalFileName);
        Task<FileAccessToken> IssueFacilityImageUploadTokenAsync(int facilityId, string originalFileName);
    }
}
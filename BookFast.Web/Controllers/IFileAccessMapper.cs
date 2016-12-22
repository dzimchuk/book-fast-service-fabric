using BookFast.Web.Contracts.Files;
using BookFast.Web.Representations;

namespace BookFast.Web.Controllers
{
    public interface IFileAccessMapper
    {
        ImageUploadToken MapFrom(FileAccessToken token);
    }
}
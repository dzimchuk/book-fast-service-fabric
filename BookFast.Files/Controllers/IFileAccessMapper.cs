using BookFast.Files.Contracts.Models;
using BookFast.Files.Models;

namespace BookFast.Files.Controllers
{
    public interface IFileAccessMapper
    {
        FileAccessTokenRepresentation MapFrom(FileAccessToken token);
    }
}

using BookFast.Files.Client.Models;
using BookFast.Web.Contracts.Files;

namespace BookFast.Web.Proxy
{
    public interface IFileAccessMapper
    {
        FileAccessToken MapFrom(FileAccessTokenRepresentation representation);
    }
}
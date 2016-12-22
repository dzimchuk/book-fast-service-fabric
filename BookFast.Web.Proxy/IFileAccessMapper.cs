using BookFast.Web.Contracts.Files;
using BookFast.Web.Proxy.Models;

namespace BookFast.Web.Proxy
{
    public interface IFileAccessMapper
    {
        FileAccessToken MapFrom(FileAccessTokenRepresentation representation);
    }
}
using AutoMapper;
using BookFast.Web.Contracts.Files;
using BookFast.Web.Controllers;
using BookFast.Web.Representations;

namespace BookFast.Web.Mappers
{
    internal class FileAccessMapper : IFileAccessMapper
    {
        private static readonly IMapper Mapper;

        static FileAccessMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<FileAccessToken, ImageUploadToken>();
            });

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }

        public ImageUploadToken MapFrom(FileAccessToken token)
        {
            return Mapper.Map<ImageUploadToken>(token);
        }
    }
}

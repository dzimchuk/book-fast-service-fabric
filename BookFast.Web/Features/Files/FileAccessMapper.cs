using AutoMapper;
using BookFast.Web.Contracts.Files;
using BookFast.Web.Features.Files.Representations;

namespace BookFast.Web.Features.Files
{
    public class FileAccessMapper
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

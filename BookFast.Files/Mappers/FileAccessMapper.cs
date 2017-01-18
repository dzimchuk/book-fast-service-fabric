using AutoMapper;
using BookFast.Files.Contracts.Models;
using BookFast.Files.Controllers;
using BookFast.Files.Models;

namespace BookFast.Files.Mappers
{
    internal class FileAccessMapper : IFileAccessMapper
    {
        private static readonly IMapper Mapper;

        static FileAccessMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<FileAccessToken, FileAccessTokenRepresentation>();
            });

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }

        public FileAccessTokenRepresentation MapFrom(FileAccessToken token)
        {
            return Mapper.Map<FileAccessTokenRepresentation>(token);
        }
    }
}

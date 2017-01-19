using AutoMapper;
using BookFast.Files.Client.Models;
using BookFast.Web.Contracts.Files;
using System;

namespace BookFast.Web.Proxy.Mappers
{
    internal class FileAccessMapper : IFileAccessMapper
    {
        private static readonly IMapper Mapper;

        static FileAccessMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<string, AccessPermission>().ConvertUsing(permission => (AccessPermission)Enum.Parse(typeof(AccessPermission), permission));
                configuration.CreateMap<FileAccessTokenRepresentation, FileAccessToken>();
            });

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }

        public FileAccessToken MapFrom(FileAccessTokenRepresentation representation)
        {
            return Mapper.Map<FileAccessToken>(representation);
        }
    }
}

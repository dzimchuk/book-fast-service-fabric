using System;
using System.Collections.Generic;
using AutoMapper;
using BookFast.Web.Contracts.Models;
using BookFast.Web.Proxy.Models;
using System.Linq;

namespace BookFast.Web.Proxy.Mappers
{
    internal class FacilityMapper : IFacilityMapper
    {
        private static readonly IMapper Mapper;

        static FacilityMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<FacilityRepresentation, Facility>()
                             .ConvertUsing(representation => new Facility
                                                             {
                                                                 Id = representation.Id ?? Guid.Empty,
                                                                 Owner = null,
                                                                 Details = new FacilityDetails
                                                                           {
                                                                               Name = representation.Name,
                                                                               Description = representation.Description,
                                                                               StreetAddress = representation.StreetAddress,
                                                                               Images = representation.Images != null ? representation.Images.ToArray() : null
                                                                           },
                                                                 Location = new Location
                                                                            {
                                                                                Latitude = representation.Latitude,
                                                                                Longitude = representation.Longitude
                                                                            },
                                                                 AccommodationCount = representation.AccommodationCount ?? 0
                                                             });
                configuration.CreateMap<FacilityDetails, FacilityData>()
                             .ForMember(d => d.Latitude, config => config.Ignore())
                             .ForMember(d => d.Longitude, config => config.Ignore())
                             .ForMember(d => d.Images, 
                                config => config.ResolveUsing<ArrayToListResolver>().FromMember(d => d.Images));
            });

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }

        public List<Facility> MapFrom(IList<FacilityRepresentation> facilities)
        {
            return Mapper.Map<List<Facility>>(facilities);
        }

        public Facility MapFrom(FacilityRepresentation facility)
        {
            return Mapper.Map<Facility>(facility);
        }

        public FacilityData MapFrom(FacilityDetails details)
        {
            return Mapper.Map<FacilityData>(details);
        }
    }
}
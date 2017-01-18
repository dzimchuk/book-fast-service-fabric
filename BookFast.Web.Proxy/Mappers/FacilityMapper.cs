using System;
using System.Collections.Generic;
using AutoMapper;
using BookFast.Web.Contracts.Models;
using System.Linq;
using BookFast.Facility.Client.Models;

namespace BookFast.Web.Proxy.Mappers
{
    internal class FacilityMapper : IFacilityMapper
    {
        private static readonly IMapper Mapper;

        static FacilityMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<FacilityRepresentation, Contracts.Models.Facility>()
                             .ConvertUsing(representation => new Contracts.Models.Facility
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

        public List<Contracts.Models.Facility> MapFrom(IList<FacilityRepresentation> facilities)
        {
            return Mapper.Map<List<Contracts.Models.Facility>>(facilities);
        }

        public Contracts.Models.Facility MapFrom(FacilityRepresentation facility)
        {
            return Mapper.Map<Contracts.Models.Facility>(facility);
        }

        public FacilityData MapFrom(FacilityDetails details)
        {
            return Mapper.Map<FacilityData>(details);
        }
    }
}
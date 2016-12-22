using System.Collections.Generic;
using AutoMapper;
using BookFast.Facility.Data.Mappers.Resolvers;
using Newtonsoft.Json;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Data.Mappers
{
    internal class FacilityMapper : IFacilityMapper
    {
        private static readonly IMapper Mapper;

        static FacilityMapper()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
                                                              {
                                                                  config.CreateMap<Contracts.Models.Facility, Models.Facility>()
                                                                        .ForMember(dm => dm.Name, c => c.MapFrom(m => m.Details.Name))
                                                                        .ForMember(dm => dm.Description, c => c.MapFrom(m => m.Details.Description))
                                                                        .ForMember(dm => dm.StreetAddress, c => c.MapFrom(m => m.Details.StreetAddress))
                                                                        .ForMember(dm => dm.Latitude, c => c.MapFrom(m => m.Location.Latitude))
                                                                        .ForMember(dm => dm.Longitude, c => c.MapFrom(m => m.Location.Longitude))
                                                                        .ForMember(dm => dm.Accommodations, c => c.Ignore())
                                                                        .ForMember(dm => dm.Images, c => c.ResolveUsing<ArrayToStringResolver>().FromMember(m => m.Details.Images))
                                                                        .ReverseMap()
                                                                        .ConvertUsing(dm => new Contracts.Models.Facility
                                                                        {
                                                                                                Id = dm.Id,
                                                                                                Owner = dm.Owner,
                                                                                                Details = new FacilityDetails
                                                                                                          {
                                                                                                              Name = dm.Name,
                                                                                                              Description = dm.Description,
                                                                                                              StreetAddress = dm.StreetAddress,
                                                                                                              Images = string.IsNullOrWhiteSpace(dm.Images) ? null : JsonConvert.DeserializeObject<string[]>(dm.Images)
                                                                                                          },
                                                                                                Location = new Location
                                                                                                           {
                                                                                                               Latitude = dm.Latitude,
                                                                                                               Longitude = dm.Longitude
                                                                                                           },
                                                                                                AccommodationCount = dm.AccommodationCount
                                                                                            });
                                                              });
            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public Contracts.Models.Facility MapFrom(Models.Facility facility)
        {
            return Mapper.Map<Contracts.Models.Facility>(facility);
        }

        public IEnumerable<Contracts.Models.Facility> MapFrom(IEnumerable<Models.Facility> facilities)
        {
            return Mapper.Map<IEnumerable<Contracts.Models.Facility>>(facilities);
        } 

        public Models.Facility MapFrom(Contracts.Models.Facility facility)
        {
            return Mapper.Map<Models.Facility>(facility);
        }
    }
}
using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Data.Mappers
{
    internal class AccommodationMapper : IAccommodationMapper
    {
        private static readonly IMapper Mapper;

        static AccommodationMapper()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
                                                              {
                                                                  config.CreateMap<Accommodation, Models.Accommodation>()
                                                                        .ForMember(dm => dm.Name, c => c.MapFrom(m => m.Details.Name))
                                                                        .ForMember(dm => dm.Description, c => c.MapFrom(m => m.Details.Description))
                                                                        .ForMember(dm => dm.RoomCount, c => c.MapFrom(m => m.Details.RoomCount))
                                                                        .ForMember(dm => dm.Facility, c => c.Ignore())
                                                                        .ForMember(dm => dm.Images, c => c.ResolveUsing<ArrayToStringResolver>())
                                                                        .ReverseMap()
                                                                        .ConvertUsing(dm => new Accommodation
                                                                                            {
                                                                                                Id = dm.Id,
                                                                                                FacilityId = dm.FacilityId,
                                                                                                Details = new AccommodationDetails
                                                                                                          {
                                                                                                              Name = dm.Name,
                                                                                                              Description = dm.Description,
                                                                                                              RoomCount = dm.RoomCount,
                                                                                                              Images = string.IsNullOrWhiteSpace(dm.Images) ? null : JsonConvert.DeserializeObject<string[]>(dm.Images)
                                                                                                          }
                                                                                            });
                                                              });
            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public Accommodation MapFrom(Models.Accommodation accommodation)
        {
            return Mapper.Map<Accommodation>(accommodation);
        }

        public IEnumerable<Accommodation> MapFrom(IEnumerable<Models.Accommodation> accommodations)
        {
            return Mapper.Map<IEnumerable<Accommodation>>(accommodations);
        } 

        public Models.Accommodation MapFrom(Accommodation accommodation)
        {
            return Mapper.Map<Models.Accommodation>(accommodation);
        }

        private class ArrayToStringResolver : IValueResolver<Accommodation, Models.Accommodation, string>
        {
            public string Resolve(Accommodation source, Models.Accommodation destination, string destMember, ResolutionContext context)
            {
                return source.Details.Images == null ? null : JsonConvert.SerializeObject(source.Details.Images);
            }
        }
    }
}
using System.Collections.Generic;
using AutoMapper;
using BookFast.Facility.Controllers;
using BookFast.Facility.Models;
using BookFast.Facility.Models.Representations;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Mappers
{
    internal class AccommodationMapper : IAccommodationMapper
    {
        private static readonly IMapper Mapper;

        static AccommodationMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
                                                              {
                                                                  configuration.CreateMap<Accommodation, AccommodationRepresentation>()
                                                                               .ForMember(vm => vm.Name, config => config.MapFrom(m => m.Details.Name))
                                                                               .ForMember(vm => vm.Description, config => config.MapFrom(m => m.Details.Description))
                                                                               .ForMember(vm => vm.RoomCount, config => config.MapFrom(m => m.Details.RoomCount))
                                                                               .ForMember(vm => vm.Images, config => config.MapFrom(m => m.Details.Images));

                                                                  configuration.CreateMap<AccommodationData, AccommodationDetails>();
                                                              });
            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public AccommodationRepresentation MapFrom(Accommodation accommodation)
        {
            return Mapper.Map<AccommodationRepresentation>(accommodation);
        }

        public IEnumerable<AccommodationRepresentation> MapFrom(IEnumerable<Accommodation> accommodations)
        {
            return Mapper.Map<IEnumerable<AccommodationRepresentation>>(accommodations);
        }

        public AccommodationDetails MapFrom(AccommodationData data)
        {
            return Mapper.Map<AccommodationDetails>(data);
        }
    }
}
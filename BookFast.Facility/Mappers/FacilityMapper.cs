using System.Collections.Generic;
using AutoMapper;
using BookFast.Facility.Controllers;
using BookFast.Facility.Models;
using BookFast.Facility.Models.Representations;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Mappers
{
    internal class FacilityMapper : IFacilityMapper
    {
        private static readonly IMapper Mapper;

        static FacilityMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
                                                              {
                                                                  configuration.CreateMap<Contracts.Models.Facility, FacilityRepresentation>()
                                                                               .ForMember(vm => vm.Name, config => config.MapFrom(m => m.Details.Name))
                                                                               .ForMember(vm => vm.Description, config => config.MapFrom(m => m.Details.Description))
                                                                               .ForMember(vm => vm.StreetAddress, config => config.MapFrom(m => m.Details.StreetAddress))
                                                                               .ForMember(vm => vm.Latitude, config => config.MapFrom(m => m.Location.Latitude))
                                                                               .ForMember(vm => vm.Longitude, config => config.MapFrom(m => m.Location.Longitude))
                                                                               .ForMember(vm => vm.Images, config => config.MapFrom(m => m.Details.Images));

                                                                  configuration.CreateMap<FacilityData, FacilityDetails>();
                                                              });
            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public FacilityRepresentation MapFrom(Contracts.Models.Facility facility)
        {
            return Mapper.Map<FacilityRepresentation>(facility);
        }

        public IEnumerable<FacilityRepresentation> MapFrom(IEnumerable<Contracts.Models.Facility> facilities)
        {
            return Mapper.Map<IEnumerable<FacilityRepresentation>>(facilities);
        }

        public FacilityDetails MapFrom(FacilityData data)
        {
            return Mapper.Map<FacilityDetails>(data);
        }
    }
}
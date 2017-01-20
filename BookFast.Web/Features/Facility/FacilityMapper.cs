using System.Collections.Generic;
using AutoMapper;
using BookFast.Web.Contracts.Models;
using BookFast.Web.Features.Facility.ViewModels;

namespace BookFast.Web.Features.Facility
{
    public class FacilityMapper
    {
        private static readonly IMapper Mapper;

        static FacilityMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
                                                              {
                                                                  configuration.CreateMap<Contracts.Models.Facility, FacilityViewModel>()
                                                                               .ForMember(vm => vm.Name, config => config.MapFrom(m => m.Details.Name))
                                                                               .ForMember(vm => vm.Description, config => config.MapFrom(m => m.Details.Description))
                                                                               .ForMember(vm => vm.StreetAddress, config => config.MapFrom(m => m.Details.StreetAddress))
                                                                               .ForMember(vm => vm.Latitude, config => config.MapFrom(m => m.Location.Latitude))
                                                                               .ForMember(vm => vm.Longitude, config => config.MapFrom(m => m.Location.Longitude))
                                                                               .ForMember(vm => vm.Images, config => config.MapFrom(m => m.Details.Images));

                                                                  configuration.CreateMap<FacilityViewModel, FacilityDetails>();
                                                              });
            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public FacilityViewModel MapFrom(Contracts.Models.Facility facility)
        {
            return Mapper.Map<FacilityViewModel>(facility);
        }

        public IEnumerable<FacilityViewModel> MapFrom(IEnumerable<Contracts.Models.Facility> facilities)
        {
            return Mapper.Map<IEnumerable<FacilityViewModel>>(facilities);
        }

        public FacilityDetails MapFrom(FacilityViewModel viewModel)
        {
            return Mapper.Map<FacilityDetails>(viewModel);
        }
    }
}
using System.Collections.Generic;
using AutoMapper;
using BookFast.Web.Contracts.Models;
using BookFast.Web.Features.Facility.ViewModels;

namespace BookFast.Web.Features.Facility
{
    public class AccommodationMapper
    {
        private static readonly IMapper Mapper;

        static AccommodationMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
                                                              {
                                                                  configuration.CreateMap<Accommodation, AccommodationViewModel>()
                                                                               .ForMember(vm => vm.Name, config => config.MapFrom(m => m.Details.Name))
                                                                               .ForMember(vm => vm.Description, config => config.MapFrom(m => m.Details.Description))
                                                                               .ForMember(vm => vm.RoomCount, config => config.MapFrom(m => m.Details.RoomCount))
                                                                               .ForMember(vm => vm.Images, config => config.MapFrom(m => m.Details.Images));

                                                                  configuration.CreateMap<AccommodationViewModel, AccommodationDetails>();
                                                              });
            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public AccommodationViewModel MapFrom(Accommodation accommodation)
        {
            return Mapper.Map<AccommodationViewModel>(accommodation);
        }

        public IEnumerable<AccommodationViewModel> MapFrom(IEnumerable<Accommodation> accommodations)
        {
            return Mapper.Map<IEnumerable<AccommodationViewModel>>(accommodations);
        }

        public AccommodationDetails MapFrom(AccommodationViewModel viewModel)
        {
            return Mapper.Map<AccommodationDetails>(viewModel);
        }
    }
}
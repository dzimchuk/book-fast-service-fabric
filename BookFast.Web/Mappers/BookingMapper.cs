using System.Collections.Generic;
using AutoMapper;
using BookFast.Web.Contracts.Models;
using BookFast.Web.Controllers;
using BookFast.Web.ViewModels;

namespace BookFast.Web.Mappers
{
    internal class BookingMapper : IBookingMapper
    {
        private static readonly IMapper Mapper;

        static BookingMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
                                                              {
                                                                  configuration.CreateMap<CreateBookingViewModel, BookingDetails>();
                                                                  configuration.CreateMap<Booking, BookingViewModel>()
                                                                               .ForMember(vm => vm.FromDate, config => config.MapFrom(m => m.Details.FromDate))
                                                                               .ForMember(vm => vm.ToDate, config => config.MapFrom(m => m.Details.ToDate));
                                                              });
            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public BookingDetails MapFrom(CreateBookingViewModel viewModel)
        {
            return Mapper.Map<BookingDetails>(viewModel);
        }

        public IEnumerable<BookingViewModel> MapFrom(IEnumerable<Booking> bookings)
        {
            return Mapper.Map<IEnumerable<BookingViewModel>>(bookings);
        }

        public BookingViewModel MapFrom(Booking booking)
        {
            return Mapper.Map<BookingViewModel>(booking);
        }
    }
}
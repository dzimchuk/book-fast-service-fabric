using System.Collections.Generic;
using AutoMapper;
using BookFast.Web.Contracts.Models;
using BookFast.Web.Features.Booking.ViewModels;

namespace BookFast.Web.Features.Booking
{
    public class BookingMapper
    {
        private static readonly IMapper Mapper;

        static BookingMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
                                                              {
                                                                  configuration.CreateMap<CreateBookingViewModel, BookingDetails>();
                                                                  configuration.CreateMap<Contracts.Models.Booking, BookingViewModel>()
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

        public IEnumerable<BookingViewModel> MapFrom(IEnumerable<Contracts.Models.Booking> bookings)
        {
            return Mapper.Map<IEnumerable<BookingViewModel>>(bookings);
        }

        public BookingViewModel MapFrom(Contracts.Models.Booking booking)
        {
            return Mapper.Map<BookingViewModel>(booking);
        }
    }
}
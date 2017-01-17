using System.Collections.Generic;
using AutoMapper;
using BookFast.Booking.Controllers;
using BookFast.Booking.Models;
using BookFast.Booking.Contracts.Models;
using BookFast.Booking.Models.Representations;

namespace BookFast.Booking.Mappers
{
    internal class BookingMapper : IBookingMapper
    {
        private static readonly IMapper Mapper;

        static BookingMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
                                                              {
                                                                  configuration.CreateMap<BookingData, BookingDetails>();
                                                                  configuration.CreateMap<Contracts.Models.Booking, BookingRepresentation>()
                                                                               .ForMember(vm => vm.FromDate, config => config.MapFrom(m => m.Details.FromDate))
                                                                               .ForMember(vm => vm.ToDate, config => config.MapFrom(m => m.Details.ToDate));
                                                              });
            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public BookingDetails MapFrom(BookingData data)
        {
            return Mapper.Map<BookingDetails>(data);
        }

        public IEnumerable<BookingRepresentation> MapFrom(IEnumerable<Contracts.Models.Booking> bookings)
        {
            return Mapper.Map<IEnumerable<BookingRepresentation>>(bookings);
        }

        public BookingRepresentation MapFrom(Contracts.Models.Booking booking)
        {
            return Mapper.Map<BookingRepresentation>(booking);
        }
    }
}
using System.Collections.Generic;
using AutoMapper;
using BookFast.Web.Contracts.Models;
using BookFast.Booking.Client.Models;

namespace BookFast.Web.Proxy.Mappers
{
    internal class BookingMapper : IBookingMapper
    {
        private static readonly IMapper Mapper;

        static BookingMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
                {
                    configuration.CreateMap<BookingDetails, BookingData>()
                                 .ConvertUsing(details => new BookingData
                                                          {
                                                              FromDate = details.FromDate.UtcDateTime,
                                                              ToDate = details.ToDate.UtcDateTime
                                                          });
                    configuration.CreateMap<BookingRepresentation, Contracts.Models.Booking>()
                                 .ConvertUsing(representation => new Contracts.Models.Booking
                                 {
                                     Id = representation.Id,
                                     AccommodationId = representation.AccommodationId,
                                     AccommodationName = representation.AccommodationName,
                                     FacilityId = representation.FacilityId,
                                     FacilityName = representation.FacilityName,
                                     StreetAddress = representation.StreetAddress,
                                     User = null,
                                     Details = new BookingDetails
                                     {
                                         FromDate = representation.FromDate,
                                         ToDate = representation.ToDate
                                     }
                                 });
                });

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }

        public BookingData MapFrom(BookingDetails details)
        {
            return Mapper.Map<BookingData>(details);
        }

        public Contracts.Models.Booking MapFrom(BookingRepresentation booking)
        {
            return Mapper.Map<Contracts.Models.Booking>(booking);
        }

        public List<Contracts.Models.Booking> MapFrom(IList<BookingRepresentation> bookings)
        {
            return Mapper.Map<List<Contracts.Models.Booking>>(bookings);
        }
    }
}
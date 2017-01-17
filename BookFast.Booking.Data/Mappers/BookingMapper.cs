using AutoMapper;

namespace BookFast.Booking.Data.Mappers
{
    internal class BookingMapper : IBookingMapper
    {
        private static readonly IMapper Mapper;

        static BookingMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
                                                              {
                                                                  configuration.CreateMap<Contracts.Models.Booking, Models.Booking>()
                                                                               .ForMember(storeModel => storeModel.FromDate, config => config.MapFrom(m => m.Details.FromDate))
                                                                               .ForMember(storeModel => storeModel.ToDate, config => config.MapFrom(m => m.Details.ToDate));
                                                              });
            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public Models.Booking MapFrom(Contracts.Models.Booking booking)
        {
            return Mapper.Map<Models.Booking>(booking);
        }
    }
}
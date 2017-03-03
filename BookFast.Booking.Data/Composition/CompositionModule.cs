using BookFast.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Booking.Business.Data;
using BookFast.Booking.Data.Mappers;

namespace BookFast.Booking.Data.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBookingDataSource, BookingDataSource>();
            services.AddScoped<IFacilityDataSource, FacilityDataSource>();

            services.AddScoped<IFacilityProxy, FacilityProxy>();

            services.AddScoped<IFacilityMapper, FacilityMapper>();
        }
    }
}

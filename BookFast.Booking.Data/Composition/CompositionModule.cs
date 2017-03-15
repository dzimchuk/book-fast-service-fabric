using BookFast.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Booking.Business.Data;
using BookFast.Booking.Data.Mappers;
using BookFast.Rest;

namespace BookFast.Booking.Data.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBookingDataSource, BookingDataSource>();
            services.AddScoped<IFacilityDataSource, FacilityDataSource>();
            
            services.AddScoped<IFacilityMapper, FacilityMapper>();

            services.AddSingleton<IAccessTokenProvider, NullAccessTokenProvider>();
            new Facility.Client.Composition.CompositionModule().AddServices(services, configuration);
        }
    }
}

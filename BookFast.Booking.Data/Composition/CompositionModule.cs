using BookFast.SeedWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Booking.Business.Data;
using BookFast.Booking.Data.Mappers;
using BookFast.Rest;
using Microsoft.Extensions.Caching.Distributed;

namespace BookFast.Booking.Data.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBookingDataSource, BookingDataSource>();
            services.AddScoped<FacilityDataSource>();
            services.AddScoped<IFacilityDataSource, CachingFacilityDataSource>(serviceProvider => 
                new CachingFacilityDataSource(serviceProvider.GetService<FacilityDataSource>(), serviceProvider.GetService<IDistributedCache>()));
            
            services.AddScoped<IFacilityMapper, FacilityMapper>();

            services.AddSingleton<IAccessTokenProvider, NullAccessTokenProvider>();
            new Facility.Client.Composition.CompositionModule().AddServices(services, configuration);

            services.AddDistributedRedisCache(redisCacheOptions =>
            {
                redisCacheOptions.Configuration = configuration["Redis:Configuration"];
                redisCacheOptions.InstanceName = configuration["Redis:InstanceName"];
            });
        }
    }
}

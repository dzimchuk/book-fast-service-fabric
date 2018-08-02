using BookFast.SeedWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Rest;
using Microsoft.Extensions.Caching.Distributed;
using BookFast.Booking.CommandStack.Data;
using BookFast.Booking.QueryStack;
using BookFast.ReliableEvents;

namespace BookFast.Booking.Data.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingQueryDataSource, BookingQueryDataSource>();

            services.AddSingleton<IReliableEventsDataSource, ReliableEventsDataSource>();

            services.AddScoped<FacilityDataSource>();
            services.AddScoped<IFacilityDataSource, CachingFacilityDataSource>(serviceProvider => 
                new CachingFacilityDataSource(serviceProvider.GetService<FacilityDataSource>(), serviceProvider.GetService<IDistributedCache>()));
            
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

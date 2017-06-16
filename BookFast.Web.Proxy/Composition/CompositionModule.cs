using BookFast.Framework;
using BookFast.Web.Contracts;
using BookFast.Web.Proxy.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace BookFast.Web.Proxy.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFacilityService, FacilityProxy>();
            services.AddScoped<IAccommodationService, AccommodationProxy>();

            services.AddSingleton<BookingProxy>();
            services.AddSingleton<IBookingService, CircuitBreakingBookingProxy>(serviceProvider =>
                new CircuitBreakingBookingProxy(serviceProvider.GetService<BookingProxy>()));

            services.AddScoped<ISearchService, SearchProxy>();
            services.AddScoped<IFileAccessProxy, FileAccessProxy>();
            
            services.AddScoped<IFacilityMapper, FacilityMapper>();
            services.AddScoped<IAccommodationMapper, AccommodationMapper>();
            services.AddScoped<IBookingMapper, BookingMapper>();
            services.AddScoped<ISearchMapper, SearchMapper>();
            services.AddScoped<IFileAccessMapper, FileAccessMapper>();

            var modules = new List<ICompositionModule>
            {
                new Facility.Client.Composition.CompositionModule(),
                new Search.Client.Composition.CompositionModule(),
                new Booking.Client.Composition.CompositionModule(),
                new Files.Client.Composition.CompositionModule()
            };

            foreach (var module in modules)
            {
                module.AddServices(services, configuration);
            }
        }
    }
}
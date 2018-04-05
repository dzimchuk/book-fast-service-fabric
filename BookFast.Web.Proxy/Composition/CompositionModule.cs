using BookFast.SeedWork;
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
            services.AddScoped<IFacilityProxy, FacilityProxy>();
            services.AddScoped<IAccommodationProxy, AccommodationProxy>();

            services.AddSingleton<BookingProxy>();
            services.AddSingleton<IBookingProxy, CircuitBreakingBookingProxy>(serviceProvider =>
                new CircuitBreakingBookingProxy(serviceProvider.GetService<BookingProxy>()));

            services.AddScoped<ISearchProxy, SearchProxy>();
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
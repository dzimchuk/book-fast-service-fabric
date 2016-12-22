using BookFast.Common.Framework;
using BookFast.Web.Contracts;
using BookFast.Web.Proxy.Mappers;
using BookFast.Web.Proxy.RestClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookFast.Web.Proxy.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFacilityService, FacilityProxy>();
            services.AddScoped<IAccommodationService, AccommodationProxy>();
            services.AddScoped<IBookingService, BookingProxy>();
            services.AddScoped<ISearchService, SearchProxy>();
            services.AddScoped<IFileAccessProxy, FileAccessProxy>();

            services.Configure<ApiOptions>(configuration.GetSection("BookFastApi"));
            services.AddScoped<IBookFastAPIFactory, BookFastAPIFactory>();

            services.AddScoped<IFacilityMapper, FacilityMapper>();
            services.AddScoped<IAccommodationMapper, AccommodationMapper>();
            services.AddScoped<IBookingMapper, BookingMapper>();
            services.AddScoped<ISearchMapper, SearchMapper>();
            services.AddScoped<IFileAccessMapper, FileAccessMapper>();
        }
    }
}
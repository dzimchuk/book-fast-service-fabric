using BookFast.Framework;
using BookFast.Web.Contracts;
using BookFast.Web.Proxy.Mappers;
using BookFast.Web.Proxy.RestClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Fabric;

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
            
            services.AddSingleton(new FabricClient());

            services.AddSingleton<ICommunicationClientFactory<CommunicationClient<IBookFastFacilityAPI>>>(
                serviceProvider => new FacilityCommunicationClientFactory(
                    new ServicePartitionResolver(() => serviceProvider.GetService<FabricClient>()), 
                    serviceProvider.GetService<IAccessTokenProvider>(), 
                    serviceProvider.GetService<IOptions<ApiOptions>>()));

            services.AddSingleton<ICommunicationClientFactory<CommunicationClient<IBookFastSearchAPI>>>(
                serviceProvider => new SearchCommunicationClientFactory(
                    new ServicePartitionResolver(() => serviceProvider.GetService<FabricClient>())));

            services.AddScoped<IBookFastAPIFactory, BookFastAPIFactory>();

            services.AddScoped<IFacilityMapper, FacilityMapper>();
            services.AddScoped<IAccommodationMapper, AccommodationMapper>();
            services.AddScoped<IBookingMapper, BookingMapper>();
            services.AddScoped<ISearchMapper, SearchMapper>();
            services.AddScoped<IFileAccessMapper, FileAccessMapper>();
        }
    }
}
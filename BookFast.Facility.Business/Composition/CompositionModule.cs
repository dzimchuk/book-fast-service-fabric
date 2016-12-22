using BookFast.Facility.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Common.Framework;
using BookFast.Facility.Contracts;

namespace BookFast.Facility.Business.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IAccommodationService, AccommodationService>();
        }
    }
}
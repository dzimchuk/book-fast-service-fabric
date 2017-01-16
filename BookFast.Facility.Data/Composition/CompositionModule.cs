using BookFast.Facility.Business;
using BookFast.Facility.Business.Data;
using BookFast.Facility.Data.Mappers;
using BookFast.Facility.Data.Models;
using BookFast.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace BookFast.Facility.Data.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookFastContext>(options => options.UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddScoped<IFacilityDataSource, FacilityDataSource>();
            services.AddScoped<IAccommodationDataSource, AccommodationDataSource>();

            services.AddScoped<IFacilityMapper, FacilityMapper>();
            services.AddScoped<IAccommodationMapper, AccommodationMapper>();

            services.Configure<SearchOptions>(configuration.GetSection("Data:Azure:Storage"));
            services.AddSingleton<ISearchIndexer, SearchIndexer>();
        }
    }
}
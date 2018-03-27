using BookFast.Facility.Business;
using BookFast.Facility.Business.Data;
using BookFast.Facility.Data.Mappers;
using BookFast.Facility.Data.Models;
using BookFast.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace BookFast.Facility.Data.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FacilityContext>(options => options.UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"], sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null); // see also https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
            })
            .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))); // disable client side evaluation, see https://docs.microsoft.com/en-us/ef/core/querying/client-eval

            services.AddScoped<IFacilityDataSource, FacilityDataSource>();
            services.AddScoped<IAccommodationDataSource, AccommodationDataSource>();

            services.AddScoped<IFacilityMapper, FacilityMapper>();
            services.AddScoped<IAccommodationMapper, AccommodationMapper>();

            services.Configure<SearchQueueOptions>(configuration.GetSection("Data:Azure:Storage"));
            services.AddSingleton<ISearchIndexer, SearchIndexer>();
        }
    }
}
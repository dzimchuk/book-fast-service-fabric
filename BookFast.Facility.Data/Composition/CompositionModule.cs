using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Data.Queries;
using BookFast.Facility.Data.Repositories;
using BookFast.Facility.QueryStack;
using BookFast.ReliableEvents;
using BookFast.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            services.AddTransient<IFacilityQueryDataSource, FacilityQueryDataSource>();
            services.AddTransient<IAccommodationQueryDataSource, AccommodationQueryDataSource>();

            services.AddTransient<IFacilityRepository, FacilityRepository>();
            services.AddTransient<IAccommodationRepository, AccommodationRepository>();

            services.AddTransient<IReliableEventsDataSource, ReliableEventsDataSource>();
        }
    }
}
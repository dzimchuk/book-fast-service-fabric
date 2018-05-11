using BookFast.SeedWork;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Search;
using Microsoft.Extensions.Options;
using BookFast.Search.Adapter.Mappers;
using BookFast.Search.Contracts;

namespace BookFast.Search.Adapter.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SearchOptions>(configuration.GetSection("Search"));

            services.AddScoped(provider => CreateSearchIndexClient(provider, true));

            services.AddScoped<ISearchResultMapper, SearchResultMapper>();
            services.AddScoped<ISearchServiceProxy>(provider => new SearchServiceProxy(CreateSearchIndexClient(provider, false), provider.GetService<ISearchResultMapper>()));

            services.AddSingleton<ISearchIndexer, SearchIndexer>();
        }

        private static ISearchIndexClient CreateSearchIndexClient(IServiceProvider provider, bool useAdminKey)
        {
            var options = provider.GetService<IOptions<SearchOptions>>();
            return new SearchIndexClient(options.Value.ServiceName,
                options.Value.IndexName, new SearchCredentials(useAdminKey ? options.Value.AdminKey : options.Value.QueryKey));
        }
    }
}

using BookFast.Framework;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Search;
using Microsoft.Extensions.Options;
using BookFast.Search.Business.Data;
using BookFast.Search.Adapter.Mappers;

namespace BookFast.Search.Adapter.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SearchOptions>(configuration.GetSection("Search"));

            services.AddScoped(provider => CreateSearchIndexClient(provider, true));

            services.AddScoped<ISearchResultMapper, SearchResultMapper>();
            services.AddScoped<ISearchDataSource>(provider => new SearchDataSource(CreateSearchIndexClient(provider, false), provider.GetService<ISearchResultMapper>()));
        }

        private static ISearchIndexClient CreateSearchIndexClient(IServiceProvider provider, bool useAdminKey)
        {
            var options = provider.GetService<IOptions<SearchOptions>>();
            return new SearchIndexClient(options.Value.ServiceName,
                options.Value.IndexName, new SearchCredentials(useAdminKey ? options.Value.AdminKey : options.Value.QueryKey));
        }
    }
}

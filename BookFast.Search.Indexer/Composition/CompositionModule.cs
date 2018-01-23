using BookFast.SeedWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookFast.Search.Indexer.Composition
{
    internal class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<QueueOptions>(configuration.GetSection("Queue"));

            services.AddSingleton<IndexerService>();
        }
    }
}

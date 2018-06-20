using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookFast.ReliableEvents
{
    public static class ServiceCollectionExtensions
    {
        public static void AddReliableEventsDispatcher(this IServiceCollection services)
        {
            services.AddSingleton<ReliableEventsDispatcher>();
            services.AddSingleton<IHostedService, ReliableEventsDispatcherService>();
        }
    }
}

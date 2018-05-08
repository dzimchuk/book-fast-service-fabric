using Microsoft.Extensions.DependencyInjection;

namespace BookFast.ReliableEvents
{
    public static class ServiceCollectionExtensions
    {
        public static void AddReliableEventsDispatcher(this IServiceCollection services)
        {
            services.AddSingleton<ReliableEventsDispatcher>();
        }
    }
}

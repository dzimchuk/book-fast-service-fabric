using BookFast.ReliableEvents.CommandStack;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookFast.ReliableEvents
{
    public static class ServiceCollectionExtensions
    {
        public static void AddReliableEventsDispatcher(this IServiceCollection services, IConfiguration configuration, IReliableEventMapper reliableEventMapper)
        {
            services.AddSingleton<ReliableEventsDispatcher>();
            services.AddSingleton<IHostedService, ReliableEventsDispatcherService>();

            services.Configure<ConnectionOptions>(configuration.GetSection("ServiceBus"));
            services.AddSingleton<INotificationHandler<EventsAvailableNotification>, NotificationPublisher>();

            services.AddSingleton(reliableEventMapper);
        }

        public static void AddCommandContext(this IServiceCollection services)
        {
            services.AddScoped<CommandContext>();
        }
    }
}

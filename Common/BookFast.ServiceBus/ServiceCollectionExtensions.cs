using BookFast.SeedWork.Modeling;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookFast.ServiceBus
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIntegrationEventPublisher(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionOptions>(configuration.GetSection("ServiceBus"));
            services.AddSingleton<TopicClientProvider>();
            services.AddScoped<INotificationHandler<IntegrationEvent>, IntegrationEventPublisher>();
        }

        public static void AddIntegrationEventReceiver(this IServiceCollection services, IConfiguration configuration, IEventMapper eventMapper)
        {
            services.Configure<ConnectionOptions>(configuration.GetSection("ServiceBus"));
            services.AddSingleton<IHostedService, IntegrationEventReceiver>();
            services.AddSingleton(eventMapper);
        }
    }
}

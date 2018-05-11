using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookFast.SeedWork
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider CreateLightInjectServiceProvider(this IServiceCollection services)
        {
            var containerOptions = new ContainerOptions { EnablePropertyInjection = false };

            var container = new ServiceContainer(containerOptions);
            container.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();

            return container.CreateServiceProvider(services);
        }
    }
}

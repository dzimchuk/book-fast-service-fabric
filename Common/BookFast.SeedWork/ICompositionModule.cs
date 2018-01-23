using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookFast.SeedWork
{
    public interface ICompositionModule
    {
        void AddServices(IServiceCollection services, IConfiguration configuration);
    }
}
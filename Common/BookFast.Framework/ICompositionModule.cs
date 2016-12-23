using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookFast.Framework
{
    public interface ICompositionModule
    {
        void AddServices(IServiceCollection services, IConfiguration configuration);
    }
}
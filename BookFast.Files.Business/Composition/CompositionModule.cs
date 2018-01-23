using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Files.Contracts;
using BookFast.SeedWork;

namespace BookFast.Files.Business.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileAccessService, FileAccessService>();
        }
    }
}

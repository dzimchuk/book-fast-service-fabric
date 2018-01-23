using BookFast.SeedWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Swagger;

namespace BookFast.Search.Composition
{
    internal class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc();
            services.AddSwashbuckle("Book Fast Search API", "v1", "BookFast.Search.xml");
        }
    }
}

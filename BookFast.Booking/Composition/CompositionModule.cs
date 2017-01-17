using BookFast.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Booking.Controllers;
using BookFast.Booking.Mappers;
using BookFast.Security.AspNetCore.Authentication;
using BookFast.Swagger;

namespace BookFast.Booking.Composition
{
    internal class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<B2CAuthenticationOptions>(configuration.GetSection("Authentication:AzureAd:B2C"));

            services.AddMvc();

            services.AddScoped<IBookingMapper, BookingMapper>();

            services.AddSwashbuckle("Book Fast Booking API", "v1", "BookFast.Booking.xml");
        }
    }
}

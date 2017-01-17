using BookFast.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Booking.Data.Models;
using Microsoft.EntityFrameworkCore;
using BookFast.Booking.Business.Data;
using BookFast.Booking.Data.Mappers;

namespace BookFast.Booking.Data.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookFastContext>(options => options.UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddScoped<IBookingDataSource, BookingDataSource>();

            services.AddScoped<IBookingMapper, BookingMapper>();
        }
    }
}

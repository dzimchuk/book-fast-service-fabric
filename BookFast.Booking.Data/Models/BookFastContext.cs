using Microsoft.EntityFrameworkCore;

namespace BookFast.Booking.Data.Models
{
    internal class BookFastContext : DbContext
    {
        public BookFastContext(DbContextOptions options) : base(options)
        {
        }

        public BookFastContext()
        {
        }
        
        public DbSet<Booking> Bookings { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=BookFast;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
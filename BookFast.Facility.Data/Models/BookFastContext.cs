using BookFast.Facility.Data.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookFast.Facility.Data.Models
{
    internal class BookFastContext : DbContext
    {
        public BookFastContext(DbContextOptions<BookFastContext> options) : base(options)
        {
        }

        // needed for tooling, alternatively one can implement IDesignTimeDbContextFactory<TContext>
        //public BookFastContext()
        //{
        //}

        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FacilityConfiguration());
            modelBuilder.ApplyConfiguration(new AccommodationConfiguration());
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=BookFast;Trusted_Connection=True;MultipleActiveResultSets=true");
        //    }
        //}
    }

    internal class BookFastContextDesignFactory : IDesignTimeDbContextFactory<BookFastContext>
    {
        public BookFastContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookFastContext>()
                .UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=BookFast;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new BookFastContext(optionsBuilder.Options);
        }
    }
}
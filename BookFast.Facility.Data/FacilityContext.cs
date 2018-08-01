using BookFast.Facility.Data.Models;
using BookFast.Facility.Data.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BookFast.Facility.Data
{
    internal class FacilityContext : DbContext
    {
        public FacilityContext(DbContextOptions<FacilityContext> options) : base(options)
        {
        }

        // needed for tooling, alternatively one can implement IDesignTimeDbContextFactory<TContext>
        //public FacilityContext()
        //{
        //}

        public DbSet<Models.Facility> Facilities { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<ReliableEvent> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FacilityConfiguration());
            modelBuilder.ApplyConfiguration(new AccommodationConfiguration());
            modelBuilder.ApplyConfiguration(new ReliableEventConfiguration());
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=BookFast;Trusted_Connection=True;MultipleActiveResultSets=true");
        //    }
        //}
    }
}
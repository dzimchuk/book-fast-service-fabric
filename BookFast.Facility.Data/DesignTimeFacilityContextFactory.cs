using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace BookFast.Facility.Data
{
    internal class DesignTimeFacilityContextFactory : IDesignTimeDbContextFactory<FacilityContext>
    {
        public FacilityContext CreateDbContext(string[] args)
        {
            var targetEnv = Environment.GetEnvironmentVariable("TargetEnv");
            if (string.IsNullOrWhiteSpace(targetEnv))
            {
                throw new ArgumentException("No target environment has been specified. Please make sure to define TargetEnv environment variable.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<FacilityContext>()
                .UseSqlServer(ConfigurationHelper.GetConnectionString(targetEnv));

            return new FacilityContext(optionsBuilder.Options);
        }
    }
}
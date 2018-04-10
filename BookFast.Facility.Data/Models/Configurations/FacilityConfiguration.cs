using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFast.Facility.Data.Models.Configurations
{
    internal class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.ToTable("Facilities", "fm");

            builder.HasKey(facility => facility.Id);
            builder.Property(facility => facility.Id).ForSqlServerUseSequenceHiLo("facilityseq", "fm");
            
            builder.Property(facility => facility.Name).IsRequired(true).HasMaxLength(320);
            builder.Property(facility => facility.Description).IsRequired(false);
            builder.Property(facility => facility.Owner).IsRequired(true);
            builder.Property(facility => facility.StreetAddress).IsRequired(false);
            builder.Property(facility => facility.Latitude).IsRequired(false);
            builder.Property(facility => facility.Longitude).IsRequired(false);
            builder.Property(facility => facility.Images).IsRequired(false);
        }
    }
}

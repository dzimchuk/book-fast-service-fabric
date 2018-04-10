using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFast.Facility.Data.Models.Configurations
{
    internal class AccommodationConfiguration : IEntityTypeConfiguration<Accommodation>
    {
        public void Configure(EntityTypeBuilder<Accommodation> builder)
        {
            builder.ToTable("Accommodations", "fm");

            builder.HasKey(accommodation => accommodation.Id);
            builder.Property(accommodation => accommodation.Id).ForSqlServerUseSequenceHiLo("accommodationseq", "fm");
            
            builder.Property(accommodation => accommodation.Name).IsRequired(true).HasMaxLength(320);
            builder.Property(accommodation => accommodation.Description).IsRequired(false);
            builder.Property(accommodation => accommodation.Images).IsRequired(false);

            builder.Property(accommodation => accommodation.RoomCount).IsRequired(true);

            builder.HasOne(accommodation => accommodation.Facility)
                .WithMany(facility => facility.Accommodations)
                .HasForeignKey(accommodation => accommodation.FacilityId);
        }
    }
}

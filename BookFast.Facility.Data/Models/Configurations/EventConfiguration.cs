using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFast.Facility.Data.Models.Configurations
{
    internal class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events", "fm");

            builder.HasKey(facility => facility.Id);
            builder.Property(facility => facility.Id).ForSqlServerUseSequenceHiLo("eventseq", "fm");

            builder.Property(facility => facility.EventName).IsRequired(true).HasMaxLength(100);
            builder.Property(facility => facility.OccurredAt).IsRequired(true);
            builder.Property(facility => facility.User).IsRequired(true).HasMaxLength(50);
            builder.Property(facility => facility.Tenant).IsRequired(true).HasMaxLength(50);
            builder.Property(facility => facility.Payload).IsRequired(true);
        }
    }
}

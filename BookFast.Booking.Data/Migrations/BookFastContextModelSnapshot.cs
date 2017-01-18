using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BookFast.Booking.Data.Models;

namespace BookFast.Booking.Data.Migrations
{
    [DbContext(typeof(BookFastContext))]
    partial class BookFastContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookFast.Booking.Data.Models.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AccommodationId");

                    b.Property<string>("AccommodationName");

                    b.Property<DateTimeOffset?>("CanceledOn");

                    b.Property<DateTimeOffset?>("CheckedInOn");

                    b.Property<Guid>("FacilityId");

                    b.Property<string>("FacilityName");

                    b.Property<DateTimeOffset>("FromDate");

                    b.Property<string>("StreetAddress");

                    b.Property<DateTimeOffset>("ToDate");

                    b.Property<string>("User");

                    b.HasKey("Id");

                    b.ToTable("Bookings");
                });
        }
    }
}

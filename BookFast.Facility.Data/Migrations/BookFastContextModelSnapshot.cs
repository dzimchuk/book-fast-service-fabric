using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BookFast.Facility.Data.Models;

namespace BookFast.Facility.Data.Migrations
{
    [DbContext(typeof(BookFastContext))]
    partial class BookFastContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookFast.Facility.Data.Models.Accommodation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<Guid>("FacilityId");

                    b.Property<string>("Images");

                    b.Property<string>("Name");

                    b.Property<int>("RoomCount");

                    b.HasKey("Id");

                    b.HasIndex("FacilityId");

                    b.ToTable("Accommodations");
                });

            modelBuilder.Entity("BookFast.Facility.Data.Models.Facility", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccommodationCount");

                    b.Property<string>("Description");

                    b.Property<string>("Images");

                    b.Property<double?>("Latitude");

                    b.Property<double?>("Longitude");

                    b.Property<string>("Name");

                    b.Property<string>("Owner");

                    b.Property<string>("StreetAddress");

                    b.HasKey("Id");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("BookFast.Facility.Data.Models.Accommodation", b =>
                {
                    b.HasOne("BookFast.Facility.Data.Models.Facility", "Facility")
                        .WithMany("Accommodations")
                        .HasForeignKey("FacilityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

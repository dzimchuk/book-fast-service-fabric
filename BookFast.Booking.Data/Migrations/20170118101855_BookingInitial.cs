using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookFast.Booking.Data.Migrations
{
    public partial class BookingInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccommodationId = table.Column<Guid>(nullable: false),
                    AccommodationName = table.Column<string>(nullable: true),
                    CanceledOn = table.Column<DateTimeOffset>(nullable: true),
                    CheckedInOn = table.Column<DateTimeOffset>(nullable: true),
                    FacilityId = table.Column<Guid>(nullable: false),
                    FacilityName = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTimeOffset>(nullable: false),
                    StreetAddress = table.Column<string>(nullable: true),
                    ToDate = table.Column<DateTimeOffset>(nullable: false),
                    User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}

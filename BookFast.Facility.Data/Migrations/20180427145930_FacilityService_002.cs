using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BookFast.Facility.Data.Migrations
{
    public partial class FacilityService_002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                schema: "fm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    EventName = table.Column<string>(maxLength: 100, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(nullable: false),
                    Payload = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateSequence(
                name: "eventseq",
                schema: "fm",
                incrementBy: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events",
                schema: "fm");

            migrationBuilder.DropSequence(
                name: "eventseq",
                schema: "fm");
        }
    }
}

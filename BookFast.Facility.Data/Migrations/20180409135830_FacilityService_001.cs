using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookFast.Facility.Data.Migrations
{
    public partial class FacilityService_001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fm");

            migrationBuilder.CreateSequence(
                name: "accommodationseq",
                schema: "fm",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "facilityseq",
                schema: "fm",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Facilities",
                schema: "fm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Images = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Name = table.Column<string>(maxLength: 320, nullable: false),
                    Owner = table.Column<string>(nullable: false),
                    StreetAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accommodations",
                schema: "fm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FacilityId = table.Column<int>(nullable: false),
                    Images = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 320, nullable: false),
                    RoomCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accommodations_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalSchema: "fm",
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_FacilityId",
                schema: "fm",
                table: "Accommodations",
                column: "FacilityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accommodations",
                schema: "fm");

            migrationBuilder.DropTable(
                name: "Facilities",
                schema: "fm");

            migrationBuilder.DropSequence(
                name: "accommodationseq",
                schema: "fm");

            migrationBuilder.DropSequence(
                name: "facilityseq",
                schema: "fm");
        }
    }
}

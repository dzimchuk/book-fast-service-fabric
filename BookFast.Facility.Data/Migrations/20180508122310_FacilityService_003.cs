using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookFast.Facility.Data.Migrations
{
    public partial class FacilityService_003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tenant",
                schema: "fm",
                table: "Events",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "User",
                schema: "fm",
                table: "Events",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tenant",
                schema: "fm",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "User",
                schema: "fm",
                table: "Events");
        }
    }
}

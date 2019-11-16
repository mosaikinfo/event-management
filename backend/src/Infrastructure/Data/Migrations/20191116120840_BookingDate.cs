using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class BookingDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "Tickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "Tickets");
        }
    }
}

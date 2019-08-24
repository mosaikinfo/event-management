using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class TicketBirthDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Tickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Tickets",
                nullable: true);
        }
    }
}

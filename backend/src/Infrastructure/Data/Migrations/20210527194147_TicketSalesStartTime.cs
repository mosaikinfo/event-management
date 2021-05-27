using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class TicketSalesStartTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TicketSalesStartTime",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketSalesStartTime",
                table: "Events");
        }
    }
}

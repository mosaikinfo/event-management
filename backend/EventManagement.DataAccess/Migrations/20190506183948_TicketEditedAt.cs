using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventManagement.DataAccess.Migrations
{
    public partial class TicketEditedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Tickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Tickets");
        }
    }
}
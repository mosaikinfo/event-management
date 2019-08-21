using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class ClientEditedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Clients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Clients");
        }
    }
}

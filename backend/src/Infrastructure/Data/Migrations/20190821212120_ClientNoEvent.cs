using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class ClientNoEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Events_EventId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_EventId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Clients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Clients",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Clients_EventId",
                table: "Clients",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Events_EventId",
                table: "Clients",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class Auditing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryType",
                table: "Tickets",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelivered",
                table: "Tickets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AuditEventLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Action = table.Column<string>(maxLength: 100, nullable: false),
                    Detail = table.Column<string>(maxLength: 1000, nullable: true),
                    Succeeded = table.Column<bool>(nullable: false),
                    TicketId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEventLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditEventLog_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEventLog_TicketId",
                table: "AuditEventLog",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEventLog");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DeliveryType",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsDelivered",
                table: "Tickets");
        }
    }
}

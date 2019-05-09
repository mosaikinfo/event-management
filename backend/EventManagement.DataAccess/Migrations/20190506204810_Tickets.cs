using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.DataAccess.Migrations
{
    public partial class Tickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TicketNumber = table.Column<string>(maxLength: 100, nullable: false),
                    TicketGuid = table.Column<Guid>(nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    Validated = table.Column<bool>(nullable: false),
                    Mail = table.Column<string>(maxLength: 254, nullable: true),
                    Phone = table.Column<string>(maxLength: 100, nullable: true),
                    PaymentStatus = table.Column<string>(maxLength: 100, nullable: false),
                    TermsAccepted = table.Column<bool>(nullable: true),
                    LastName = table.Column<string>(maxLength: 300, nullable: true),
                    FirstName = table.Column<string>(maxLength: 300, nullable: true),
                    Age = table.Column<int>(nullable: true),
                    Address = table.Column<string>(maxLength: 1000, nullable: true),
                    RoomNumber = table.Column<string>(maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    EditedAt = table.Column<DateTime>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    EditorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.UniqueConstraint("AK_Tickets_TicketGuid", x => x.TicketGuid);
                    table.UniqueConstraint("AK_Tickets_TicketNumber", x => x.TicketNumber);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_EditorId",
                        column: x => x.EditorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatorId",
                table: "Tickets",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EditorId",
                table: "Tickets",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // reset database if it already existed before.
            migrationBuilder.Sql("DROP TABLE IF EXISTS dbo.Tickets");
            migrationBuilder.Sql("DROP TABLE IF EXISTS dbo.Users");
            migrationBuilder.Sql("DROP TABLE IF EXISTS dbo.TicketTypes");
            migrationBuilder.Sql("DROP TABLE IF EXISTS dbo.Events");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    EntranceTime = table.Column<DateTime>(nullable: true),
                    Location = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    Username = table.Column<string>(maxLength: 300, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 300, nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<string>(maxLength: 300, nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(5, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketTypes_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TicketNumber = table.Column<string>(maxLength: 100, nullable: false),
                    TicketSecret = table.Column<string>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false),
                    TicketTypeId = table.Column<Guid>(nullable: false),
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
                    CreatorId = table.Column<Guid>(nullable: true),
                    EditorId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.UniqueConstraint("AK_Tickets_TicketNumber", x => x.TicketNumber);
                    table.UniqueConstraint("AK_Tickets_TicketSecret", x => x.TicketSecret);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketTypeId",
                table: "Tickets",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTypes_EventId",
                table: "TicketTypes",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}

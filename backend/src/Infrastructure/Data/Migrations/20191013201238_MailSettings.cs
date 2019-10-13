using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class MailSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MailSettingsId",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MailSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SmtpHost = table.Column<string>(maxLength: 300, nullable: false),
                    SmtpPort = table.Column<int>(nullable: false),
                    SenderAddress = table.Column<string>(maxLength: 300, nullable: false),
                    Subject = table.Column<string>(maxLength: 300, nullable: false),
                    Body = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_MailSettingsId",
                table: "Events",
                column: "MailSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_MailSettings_MailSettingsId",
                table: "Events",
                column: "MailSettingsId",
                principalTable: "MailSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_MailSettings_MailSettingsId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "MailSettings");

            migrationBuilder.DropIndex(
                name: "IX_Events_MailSettingsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MailSettingsId",
                table: "Events");
        }
    }
}

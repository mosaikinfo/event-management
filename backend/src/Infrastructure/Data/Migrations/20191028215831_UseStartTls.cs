using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class UseStartTls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_MailSettingsId",
                table: "Events");

            migrationBuilder.AddColumn<bool>(
                name: "UseStartTls",
                table: "MailSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Events_MailSettingsId",
                table: "Events",
                column: "MailSettingsId",
                unique: true,
                filter: "[MailSettingsId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_MailSettingsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UseStartTls",
                table: "MailSettings");

            migrationBuilder.CreateIndex(
                name: "IX_Events_MailSettingsId",
                table: "Events",
                column: "MailSettingsId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class DemoEmailRecipients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableDemoMode",
                table: "MailSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DemoEmailRecipients",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MailSettingsId = table.Column<Guid>(nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoEmailRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoEmailRecipients_MailSettings_MailSettingsId",
                        column: x => x.MailSettingsId,
                        principalTable: "MailSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoEmailRecipients_MailSettingsId",
                table: "DemoEmailRecipients",
                column: "MailSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoEmailRecipients");

            migrationBuilder.DropColumn(
                name: "EnableDemoMode",
                table: "MailSettings");
        }
    }
}

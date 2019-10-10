using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class EventDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HomepageUrl",
                table: "Events",
                maxLength: 2083,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2083,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Events",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Events",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "Events",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Events",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Host",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "HomepageUrl",
                table: "Events",
                maxLength: 2083,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2083);
        }
    }
}

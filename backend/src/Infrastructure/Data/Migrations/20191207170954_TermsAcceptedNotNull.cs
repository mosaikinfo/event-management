using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class TermsAcceptedNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update Tickets set TermsAccepted = 0 where TermsAccepted is null");

            migrationBuilder.AlterColumn<bool>(
                name: "TermsAccepted",
                table: "Tickets",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "TermsAccepted",
                table: "Tickets",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
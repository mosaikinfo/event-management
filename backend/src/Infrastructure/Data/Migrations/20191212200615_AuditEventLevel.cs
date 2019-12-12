using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Infrastructure.Data.Migrations
{
    public partial class AuditEventLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "AuditEventLog",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("update dbo.AuditEventLog set Level = 'success' where Succeeded = 1");
            migrationBuilder.Sql("update dbo.AuditEventLog set Level = 'fail' where Succeeded = 0");

            migrationBuilder.DropColumn(
                name: "Succeeded",
                table: "AuditEventLog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Succeeded",
                table: "AuditEventLog",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("update dbo.AuditEventLog set Succeeded = 1 where Level = 'success'");
            migrationBuilder.Sql("update dbo.AuditEventLog set Succeeded = 0 where Level = 'fail'");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "AuditEventLog");
        }
    }
}

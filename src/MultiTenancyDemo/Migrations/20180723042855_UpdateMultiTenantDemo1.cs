using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiTenancyDemo.Migrations
{
    public partial class UpdateMultiTenantDemo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostName",
                table: "Tenants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostName",
                table: "Tenants");
        }
    }
}

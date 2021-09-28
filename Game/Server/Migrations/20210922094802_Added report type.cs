using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class Addedreporttype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportType",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "Reports");
        }
    }
}

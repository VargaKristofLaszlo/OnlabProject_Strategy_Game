using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class updatedUnitTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DefensePoint",
                table: "Units",
                newName: "UnitType");

            migrationBuilder.AddColumn<int>(
                name: "ArcherDefensePoint",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CavalryDefensePoint",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InfantryDefensePoint",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArcherDefensePoint",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CavalryDefensePoint",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "InfantryDefensePoint",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "UnitType",
                table: "Units",
                newName: "DefensePoint");
        }
    }
}

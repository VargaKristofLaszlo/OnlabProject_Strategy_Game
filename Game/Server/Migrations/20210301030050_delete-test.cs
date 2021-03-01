using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class deletetest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitsInCities_Barracks_BarrackId",
                table: "UnitsInCities");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitsInCities_Units_UnitId",
                table: "UnitsInCities");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitsInCities_Barracks_BarrackId",
                table: "UnitsInCities",
                column: "BarrackId",
                principalTable: "Barracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitsInCities_Units_UnitId",
                table: "UnitsInCities",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitsInCities_Barracks_BarrackId",
                table: "UnitsInCities");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitsInCities_Units_UnitId",
                table: "UnitsInCities");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitsInCities_Barracks_BarrackId",
                table: "UnitsInCities",
                column: "BarrackId",
                principalTable: "Barracks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitsInCities_Units_UnitId",
                table: "UnitsInCities",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}

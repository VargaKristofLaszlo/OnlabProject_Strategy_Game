using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class barrackRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_ResourceProductions_SilverProductionId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_ResourceProductions_StoneProductionId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_ResourceProductions_WoodProductionId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitsInCities_Barracks_BarrackId",
                table: "UnitsInCities");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitsInCities_Units_UnitId",
                table: "UnitsInCities");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_ResourceProductions_SilverProductionId",
                table: "Cities",
                column: "SilverProductionId",
                principalTable: "ResourceProductions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_ResourceProductions_StoneProductionId",
                table: "Cities",
                column: "StoneProductionId",
                principalTable: "ResourceProductions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_ResourceProductions_WoodProductionId",
                table: "Cities",
                column: "WoodProductionId",
                principalTable: "ResourceProductions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitsInCities_Barracks_BarrackId",
                table: "UnitsInCities",
                column: "BarrackId",
                principalTable: "Barracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitsInCities_Units_UnitId",
                table: "UnitsInCities",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_ResourceProductions_SilverProductionId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_ResourceProductions_StoneProductionId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_ResourceProductions_WoodProductionId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitsInCities_Barracks_BarrackId",
                table: "UnitsInCities");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitsInCities_Units_UnitId",
                table: "UnitsInCities");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_ResourceProductions_SilverProductionId",
                table: "Cities",
                column: "SilverProductionId",
                principalTable: "ResourceProductions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_ResourceProductions_StoneProductionId",
                table: "Cities",
                column: "StoneProductionId",
                principalTable: "ResourceProductions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_ResourceProductions_WoodProductionId",
                table: "Cities",
                column: "WoodProductionId",
                principalTable: "ResourceProductions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

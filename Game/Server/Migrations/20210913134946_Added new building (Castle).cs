using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class AddednewbuildingCastle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Castles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CoinCount = table.Column<int>(type: "int", nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    BuildingName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingCostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Castles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Castles_BuildingUpgradeCosts_BuildingCostId",
                        column: x => x.BuildingCostId,
                        principalTable: "BuildingUpgradeCosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Castles_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Castles_BuildingCostId",
                table: "Castles",
                column: "BuildingCostId");

            migrationBuilder.CreateIndex(
                name: "IX_Castles_CityId",
                table: "Castles",
                column: "CityId",
                unique: true,
                filter: "[CityId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Castles");
        }
    }
}

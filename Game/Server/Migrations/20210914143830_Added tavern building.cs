using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class Addedtavernbuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoinCount",
                table: "Castles",
                newName: "MaximumCoinCount");

            migrationBuilder.AddColumn<int>(
                name: "Loyalty",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailableCoinCount",
                table: "Castles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Taverns",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpyCount = table.Column<int>(type: "int", nullable: false),
                    MaximumSpyCount = table.Column<int>(type: "int", nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    BuildingName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingCostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taverns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taverns_BuildingUpgradeCosts_BuildingCostId",
                        column: x => x.BuildingCostId,
                        principalTable: "BuildingUpgradeCosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Taverns_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Taverns_BuildingCostId",
                table: "Taverns",
                column: "BuildingCostId");

            migrationBuilder.CreateIndex(
                name: "IX_Taverns_CityId",
                table: "Taverns",
                column: "CityId",
                unique: true,
                filter: "[CityId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Taverns");

            migrationBuilder.DropColumn(
                name: "Loyalty",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "AvailableCoinCount",
                table: "Castles");

            migrationBuilder.RenameColumn(
                name: "MaximumCoinCount",
                table: "Castles",
                newName: "CoinCount");
        }
    }
}

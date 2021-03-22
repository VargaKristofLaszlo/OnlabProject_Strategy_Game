using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class updatedRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_AttackerId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_DefenderId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_UnitsInCities_AttackingForcesId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_UnitsInCities_DefendingForcesId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_UnitsInCities_SurvivedAttackingForcesId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_UnitsInCities_SurvivedDefendingForcesId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_AttackerId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_AttackingForcesId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_DefenderId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_DefendingForcesId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_SurvivedAttackingForcesId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_SurvivedDefendingForcesId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AttackerId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AttackingForcesId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DefenderId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DefendingForcesId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SurvivedAttackingForcesId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SurvivedDefendingForcesId",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "ArcherAttackerCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArcherAttackerCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArcherDefenderCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArcherDefenderCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Attacker",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttackerCityName",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AxeFighterAttackerCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AxeFighterAttackerCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AxeFighterDefenderCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AxeFighterDefenderCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Defender",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefendingCityName",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeavyCavalryAttackerCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HeavyCavalryAttackerCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HeavyCavalryDefenderCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HeavyCavalryDefenderCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LightCavalryAttackerCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LightCavalryAttackerCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LightCavalryDefenderCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LightCavalryDefenderCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MountedArcherAttackerCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MountedArcherAttackerCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MountedArcherDefenderCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MountedArcherDefenderCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpearmanAttackerCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpearmanAttackerCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpearmanDefenderCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpearmanDefenderCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SwordsmanAttackerCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SwordsmanAttackerCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SwordsmanDefenderCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SwordsmanDefenderCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArcherAttackerCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ArcherAttackerCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ArcherDefenderCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ArcherDefenderCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Attacker",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AttackerCityName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AxeFighterAttackerCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AxeFighterAttackerCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AxeFighterDefenderCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AxeFighterDefenderCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Defender",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DefendingCityName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "HeavyCavalryAttackerCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "HeavyCavalryAttackerCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "HeavyCavalryDefenderCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "HeavyCavalryDefenderCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LightCavalryAttackerCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LightCavalryAttackerCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LightCavalryDefenderCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LightCavalryDefenderCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MountedArcherAttackerCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MountedArcherAttackerCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MountedArcherDefenderCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MountedArcherDefenderCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SpearmanAttackerCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SpearmanAttackerCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SpearmanDefenderCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SpearmanDefenderCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SwordsmanAttackerCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SwordsmanAttackerCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SwordsmanDefenderCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SwordsmanDefenderCountBefore",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "AttackerId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttackingForcesId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefenderId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefendingForcesId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurvivedAttackingForcesId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurvivedDefendingForcesId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AttackerId",
                table: "Reports",
                column: "AttackerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AttackingForcesId",
                table: "Reports",
                column: "AttackingForcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DefenderId",
                table: "Reports",
                column: "DefenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DefendingForcesId",
                table: "Reports",
                column: "DefendingForcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SurvivedAttackingForcesId",
                table: "Reports",
                column: "SurvivedAttackingForcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SurvivedDefendingForcesId",
                table: "Reports",
                column: "SurvivedDefendingForcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_AttackerId",
                table: "Reports",
                column: "AttackerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_DefenderId",
                table: "Reports",
                column: "DefenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_UnitsInCities_AttackingForcesId",
                table: "Reports",
                column: "AttackingForcesId",
                principalTable: "UnitsInCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_UnitsInCities_DefendingForcesId",
                table: "Reports",
                column: "DefendingForcesId",
                principalTable: "UnitsInCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_UnitsInCities_SurvivedAttackingForcesId",
                table: "Reports",
                column: "SurvivedAttackingForcesId",
                principalTable: "UnitsInCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_UnitsInCities_SurvivedDefendingForcesId",
                table: "Reports",
                column: "SurvivedDefendingForcesId",
                principalTable: "UnitsInCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

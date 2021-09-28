using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class Spyreportresourcescreationtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attacker",
                table: "SpyReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "SpyReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Silver",
                table: "SpyReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stone",
                table: "SpyReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wood",
                table: "SpyReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attacker",
                table: "SpyReports");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "SpyReports");

            migrationBuilder.DropColumn(
                name: "Silver",
                table: "SpyReports");

            migrationBuilder.DropColumn(
                name: "Stone",
                table: "SpyReports");

            migrationBuilder.DropColumn(
                name: "Wood",
                table: "SpyReports");
        }
    }
}

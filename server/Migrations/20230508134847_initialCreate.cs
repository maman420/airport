using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LegLocation = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "flightsLogger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Leg = table.Column<int>(type: "INTEGER", nullable: false),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false),
                    Enter = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Exit = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flightsLogger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_flightsLogger_flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_flightsLogger_FlightId",
                table: "flightsLogger",
                column: "FlightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flightsLogger");

            migrationBuilder.DropTable(
                name: "flights");
        }
    }
}

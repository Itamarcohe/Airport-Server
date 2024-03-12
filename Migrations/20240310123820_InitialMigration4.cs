using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Legs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CrossingTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyLegs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyLegs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyLegs_Legs_CurrentId",
                        column: x => x.CurrentId,
                        principalTable: "Legs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    LegId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_MyLegs_LegId",
                        column: x => x.LegId,
                        principalTable: "MyLegs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    LegId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    In = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Out = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logs_Legs_LegId",
                        column: x => x.LegId,
                        principalTable: "Legs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_LegId",
                table: "Flights",
                column: "LegId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_FlightId",
                table: "Logs",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_LegId",
                table: "Logs",
                column: "LegId");

            migrationBuilder.CreateIndex(
                name: "IX_MyLegs_CurrentId",
                table: "MyLegs",
                column: "CurrentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "MyLegs");

            migrationBuilder.DropTable(
                name: "Legs");
        }
    }
}

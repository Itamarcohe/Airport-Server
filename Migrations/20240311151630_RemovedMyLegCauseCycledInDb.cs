using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportServer.Migrations
{
    /// <inheritdoc />
    public partial class RemovedMyLegCauseCycledInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_MyLegs_LegId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Legs_MyLegs_MyLegId",
                table: "Legs");

            migrationBuilder.DropTable(
                name: "MyLegs");

            migrationBuilder.DropIndex(
                name: "IX_Legs_MyLegId",
                table: "Legs");

            migrationBuilder.DropColumn(
                name: "MyLegId",
                table: "Legs");

            migrationBuilder.AddColumn<string>(
                name: "NextLegs",
                table: "Legs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Legs_LegId",
                table: "Flights",
                column: "LegId",
                principalTable: "Legs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Legs_LegId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "NextLegs",
                table: "Legs");

            migrationBuilder.AddColumn<int>(
                name: "MyLegId",
                table: "Legs",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Legs_MyLegId",
                table: "Legs",
                column: "MyLegId");

            migrationBuilder.CreateIndex(
                name: "IX_MyLegs_CurrentId",
                table: "MyLegs",
                column: "CurrentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_MyLegs_LegId",
                table: "Flights",
                column: "LegId",
                principalTable: "MyLegs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Legs_MyLegs_MyLegId",
                table: "Legs",
                column: "MyLegId",
                principalTable: "MyLegs",
                principalColumn: "Id");
        }
    }
}

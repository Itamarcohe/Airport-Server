using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportServer.Migrations
{
    /// <inheritdoc />
    public partial class UsingLegsList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LegId",
                table: "Legs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Legs_LegId",
                table: "Legs",
                column: "LegId");

            migrationBuilder.AddForeignKey(
                name: "FK_Legs_Legs_LegId",
                table: "Legs",
                column: "LegId",
                principalTable: "Legs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Legs_Legs_LegId",
                table: "Legs");

            migrationBuilder.DropIndex(
                name: "IX_Legs_LegId",
                table: "Legs");

            migrationBuilder.DropColumn(
                name: "LegId",
                table: "Legs");
        }
    }
}

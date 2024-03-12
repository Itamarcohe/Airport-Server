using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportServer.Migrations
{
    /// <inheritdoc />
    public partial class Upd3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyLegId",
                table: "Legs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Legs_MyLegId",
                table: "Legs",
                column: "MyLegId");

            migrationBuilder.AddForeignKey(
                name: "FK_Legs_MyLegs_MyLegId",
                table: "Legs",
                column: "MyLegId",
                principalTable: "MyLegs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Legs_MyLegs_MyLegId",
                table: "Legs");

            migrationBuilder.DropIndex(
                name: "IX_Legs_MyLegId",
                table: "Legs");

            migrationBuilder.DropColumn(
                name: "MyLegId",
                table: "Legs");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportServer.Migrations
{
    /// <inheritdoc />
    public partial class AddingAllLegsMyLegs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_MyLegs_LegId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "LegId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_MyLegs_LegId",
                table: "Flights",
                column: "LegId",
                principalTable: "MyLegs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_MyLegs_LegId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "LegId",
                table: "Flights",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_MyLegs_LegId",
                table: "Flights",
                column: "LegId",
                principalTable: "MyLegs",
                principalColumn: "Id");
        }
    }
}

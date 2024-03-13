using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportServer.Migrations
{
    /// <inheritdoc />
    public partial class BackToIntNext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Legs_Legs_LegId",
                table: "Legs");

            migrationBuilder.DropTable(
                name: "NextLeg");

            migrationBuilder.DropIndex(
                name: "IX_Legs_LegId",
                table: "Legs");

            migrationBuilder.DropColumn(
                name: "LegId",
                table: "Legs");

            migrationBuilder.AddColumn<string>(
                name: "NextLegs",
                table: "Legs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextLegs",
                table: "Legs");

            migrationBuilder.AddColumn<int>(
                name: "LegId",
                table: "Legs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NextLeg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextLeg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NextLeg_Legs_LegId",
                        column: x => x.LegId,
                        principalTable: "Legs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Legs_LegId",
                table: "Legs",
                column: "LegId");

            migrationBuilder.CreateIndex(
                name: "IX_NextLeg_LegId",
                table: "NextLeg",
                column: "LegId");

            migrationBuilder.AddForeignKey(
                name: "FK_Legs_Legs_LegId",
                table: "Legs",
                column: "LegId",
                principalTable: "Legs",
                principalColumn: "Id");
        }
    }
}

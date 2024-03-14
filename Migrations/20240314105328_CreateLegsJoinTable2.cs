using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateLegsJoinTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegsJoinTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromLegId = table.Column<int>(type: "int", nullable: true),
                    ToLegId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegsJoinTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegsJoinTable_Legs_FromLegId",
                        column: x => x.FromLegId,
                        principalTable: "Legs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegsJoinTable_Legs_ToLegId",
                        column: x => x.ToLegId,
                        principalTable: "Legs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegsJoinTable_FromLegId",
                table: "LegsJoinTable",
                column: "FromLegId");

            migrationBuilder.CreateIndex(
                name: "IX_LegsJoinTable_ToLegId",
                table: "LegsJoinTable",
                column: "ToLegId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegsJoinTable");
        }
    }
}

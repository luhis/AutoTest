using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoTest.Persistence.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Penalty",
                columns: table => new
                {
                    PenaltyId = table.Column<ulong>(nullable: false),
                    PenaltyType = table.Column<int>(nullable: false),
                    TestRunId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalty", x => x.PenaltyId);
                    table.ForeignKey(
                        name: "FK_Penalty_TestRuns_TestRunId",
                        column: x => x.TestRunId,
                        principalTable: "TestRuns",
                        principalColumn: "TestRunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Penalty_TestRunId",
                table: "Penalty",
                column: "TestRunId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Penalty");
        }
    }
}

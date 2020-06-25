using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoTest.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<ulong>(nullable: false),
                    ClubName = table.Column<string>(nullable: false),
                    ClubPaymentAddress = table.Column<string>(nullable: false),
                    Website = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<ulong>(nullable: false),
                    GivenName = table.Column<string>(nullable: false),
                    FamilyName = table.Column<string>(nullable: false),
                    MsaLicense = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AdminEmail",
                columns: table => new
                {
                    AdminEmailId = table.Column<ulong>(nullable: false),
                    ClubId = table.Column<ulong>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminEmail", x => x.AdminEmailId);
                    table.ForeignKey(
                        name: "FK_AdminEmail_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<ulong>(nullable: false),
                    ClubId = table.Column<ulong>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entrants",
                columns: table => new
                {
                    EntrantId = table.Column<ulong>(nullable: false),
                    GivenName = table.Column<string>(nullable: false),
                    FamilyName = table.Column<string>(nullable: false),
                    Registration = table.Column<string>(nullable: false),
                    Class = table.Column<string>(nullable: false),
                    EventId = table.Column<ulong>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrants", x => x.EntrantId);
                    table.ForeignKey(
                        name: "FK_Entrants_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    TestId = table.Column<ulong>(nullable: false),
                    EventId = table.Column<ulong>(nullable: false),
                    Ordinal = table.Column<uint>(nullable: false),
                    MapLocation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.TestId);
                    table.ForeignKey(
                        name: "FK_Tests_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestRuns",
                columns: table => new
                {
                    TestRunId = table.Column<ulong>(nullable: false),
                    TestId = table.Column<ulong>(nullable: false),
                    TimeInMS = table.Column<ulong>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    EntrantId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRuns", x => x.TestRunId);
                    table.ForeignKey(
                        name: "FK_TestRuns_Entrants_EntrantId",
                        column: x => x.EntrantId,
                        principalTable: "Entrants",
                        principalColumn: "EntrantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestRuns_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminEmail_ClubId",
                table: "AdminEmail",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrants_EventId",
                table: "Entrants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ClubId",
                table: "Events",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_TestRuns_EntrantId",
                table: "TestRuns",
                column: "EntrantId");

            migrationBuilder.CreateIndex(
                name: "IX_TestRuns_TestId",
                table: "TestRuns",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_EventId",
                table: "Tests",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminEmail");

            migrationBuilder.DropTable(
                name: "TestRuns");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Entrants");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Clubs");
        }
    }
}

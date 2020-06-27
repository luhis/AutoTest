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
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<ulong>(nullable: false),
                    GivenName = table.Column<string>(nullable: false),
                    FamilyName = table.Column<string>(nullable: false),
                    MsaLicense = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
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
                name: "Entrant",
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
                    table.PrimaryKey("PK_Entrant", x => x.EntrantId);
                    table.ForeignKey(
                        name: "FK_Entrant_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    TestId = table.Column<ulong>(nullable: false),
                    EventId = table.Column<ulong>(nullable: false),
                    Ordinal = table.Column<uint>(nullable: false),
                    MapLocation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.TestId);
                    table.ForeignKey(
                        name: "FK_Test_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestRun",
                columns: table => new
                {
                    TestRunId = table.Column<ulong>(nullable: false),
                    TestId = table.Column<ulong>(nullable: false),
                    TimeInMS = table.Column<ulong>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    Entrant = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRun", x => x.TestRunId);
                    table.ForeignKey(
                        name: "FK_TestRun_Entrant_Entrant",
                        column: x => x.Entrant,
                        principalTable: "Entrant",
                        principalColumn: "EntrantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestRun_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminEmail_ClubId",
                table: "AdminEmail",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrant_EventId",
                table: "Entrant",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ClubId",
                table: "Events",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_EventId",
                table: "Test",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_TestRun_Entrant",
                table: "TestRun",
                column: "Entrant");

            migrationBuilder.CreateIndex(
                name: "IX_TestRun_TestId",
                table: "TestRun",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminEmail");

            migrationBuilder.DropTable(
                name: "TestRun");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Entrant");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Clubs");
        }
    }
}

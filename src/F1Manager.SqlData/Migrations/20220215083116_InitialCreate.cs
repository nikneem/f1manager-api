using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Manager.SqlData.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChassisPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    QualificationPoints = table.Column<int>(type: "int", nullable: false),
                    RacePoints = table.Column<int>(type: "int", nullable: false),
                    SprintRacePoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChassisPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Circuits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstGrandPrix = table.Column<int>(type: "int", nullable: false),
                    NumberOfLaps = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(5,3)", nullable: false),
                    RaceDistance = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    LapRecord = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Circuits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DriverPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    QualificationPoints = table.Column<int>(type: "int", nullable: false),
                    RacePoints = table.Column<int>(type: "int", nullable: false),
                    SprintRacePoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnginePoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    QualificationPoints = table.Column<int>(type: "int", nullable: false),
                    RacePoints = table.Column<int>(type: "int", nullable: false),
                    SprintRacePoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnginePoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Money = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaceEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    CircuitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Practice01 = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Practice02 = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Practice03 = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Qualification = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SprintRace = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Race = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaceEvents_Circuits_CircuitId",
                        column: x => x.CircuitId,
                        principalTable: "Circuits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamChassis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChassisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsGained = table.Column<int>(type: "int", nullable: false),
                    WarnOffPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    BoughtFor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SoldFor = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    BoughtOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SoldOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamChassis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamChassis_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamDrivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsGained = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    IsFirstDriver = table.Column<bool>(type: "bit", nullable: false),
                    BoughtOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SoldOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamDrivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamDrivers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamEngine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsGained = table.Column<int>(type: "int", nullable: false),
                    WarnOffPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    BoughtFor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SoldFor = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    BoughtOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SoldOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamEngine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamEngine_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaceDriverResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaceWeekendId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChassisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QualificationResult = table.Column<int>(type: "int", nullable: false),
                    SprintRaceResult = table.Column<int>(type: "int", nullable: true),
                    RaceResult = table.Column<int>(type: "int", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceDriverResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaceDriverResults_RaceEvents_RaceWeekendId",
                        column: x => x.RaceWeekendId,
                        principalTable: "RaceEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaceResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaceWeekendId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FastestLapDriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasHalfPoints = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaceResults_RaceEvents_RaceWeekendId",
                        column: x => x.RaceWeekendId,
                        principalTable: "RaceEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceDriverResults_RaceWeekendId",
                table: "RaceDriverResults",
                column: "RaceWeekendId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceEvents_CircuitId",
                table: "RaceEvents",
                column: "CircuitId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceResults_RaceWeekendId",
                table: "RaceResults",
                column: "RaceWeekendId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamChassis_TeamId",
                table: "TeamChassis",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamDrivers_TeamId",
                table: "TeamDrivers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamEngine_TeamId",
                table: "TeamEngine",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChassisPoints");

            migrationBuilder.DropTable(
                name: "DriverPoints");

            migrationBuilder.DropTable(
                name: "EnginePoints");

            migrationBuilder.DropTable(
                name: "RaceDriverResults");

            migrationBuilder.DropTable(
                name: "RaceResults");

            migrationBuilder.DropTable(
                name: "TeamChassis");

            migrationBuilder.DropTable(
                name: "TeamDrivers");

            migrationBuilder.DropTable(
                name: "TeamEngine");

            migrationBuilder.DropTable(
                name: "RaceEvents");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Circuits");
        }
    }
}

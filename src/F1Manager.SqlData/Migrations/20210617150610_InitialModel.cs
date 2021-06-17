using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace F1Manager.SqlData.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "TeamChassis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChassisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointsGained = table.Column<int>(type: "int", nullable: false),
                    WarnOffPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointsGained = table.Column<int>(type: "int", nullable: false),
                    WarnOffPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                name: "Circuits");

            migrationBuilder.DropTable(
                name: "TeamChassis");

            migrationBuilder.DropTable(
                name: "TeamDrivers");

            migrationBuilder.DropTable(
                name: "TeamEngine");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}

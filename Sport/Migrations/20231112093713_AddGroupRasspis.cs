using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Migrations
{
    public partial class AddGroupRasspis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupClassesRaspis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportsId = table.Column<int>(type: "int", nullable: true),
                    CoachId = table.Column<int>(type: "int", nullable: true),
                    HallId = table.Column<int>(type: "int", nullable: true),
                    ReservedPlace = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    StatusGroup = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupClassesRaspis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupClassesRaspis_Coach_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coach",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupClassesRaspis_Hall_HallId",
                        column: x => x.HallId,
                        principalTable: "Hall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupClassesRaspis_Sports_SportsId",
                        column: x => x.SportsId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupClassesRaspis_CoachId",
                table: "GroupClassesRaspis",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupClassesRaspis_HallId",
                table: "GroupClassesRaspis",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupClassesRaspis_SportsId",
                table: "GroupClassesRaspis",
                column: "SportsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupClassesRaspis");
        }
    }
}

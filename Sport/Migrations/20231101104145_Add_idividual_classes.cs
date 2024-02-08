using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Migrations
{
    public partial class Add_idividual_classes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RaspisIndividual",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportsId = table.Column<int>(type: "int", nullable: true),
                    CoachId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HallId = table.Column<int>(type: "int", nullable: true),
                    Status1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaspisIndividual", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaspisIndividual_Coach_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coach",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RaspisIndividual_Hall_HallId",
                        column: x => x.HallId,
                        principalTable: "Hall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RaspisIndividual_Sports_SportsId",
                        column: x => x.SportsId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassesIndividual",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentsId = table.Column<int>(type: "int", nullable: true),
                    RaspisIndividualId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassesIndividual", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassesIndividual_RaspisIndividual_RaspisIndividualId",
                        column: x => x.RaspisIndividualId,
                        principalTable: "RaspisIndividual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassesIndividual_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassesIndividual_RaspisIndividualId",
                table: "ClassesIndividual",
                column: "RaspisIndividualId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassesIndividual_StudentsId",
                table: "ClassesIndividual",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_RaspisIndividual_CoachId",
                table: "RaspisIndividual",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_RaspisIndividual_HallId",
                table: "RaspisIndividual",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_RaspisIndividual_SportsId",
                table: "RaspisIndividual",
                column: "SportsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassesIndividual");

            migrationBuilder.DropTable(
                name: "RaspisIndividual");
        }
    }
}

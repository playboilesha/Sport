﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Migrations
{
    public partial class addcheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Check_user");

            migrationBuilder.AddColumn<int>(
                name: "Check",
                table: "ClassesGroup",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Check",
                table: "ClassesGroup");

            migrationBuilder.CreateTable(
                name: "Check_user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassesGroupId = table.Column<int>(type: "int", nullable: true),
                    StudentsId = table.Column<int>(type: "int", nullable: true),
                    check = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Check_user", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Check_user_ClassesGroup_ClassesGroupId",
                        column: x => x.ClassesGroupId,
                        principalTable: "ClassesGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Check_user_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Check_user_ClassesGroupId",
                table: "Check_user",
                column: "ClassesGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Check_user_StudentsId",
                table: "Check_user",
                column: "StudentsId");
        }
    }
}

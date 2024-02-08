using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Migrations
{
    public partial class addtableClassesGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassesGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentsId = table.Column<int>(type: "int", nullable: true),
                    GroupClassesRaspisId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassesGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassesGroup_GroupClassesRaspis_GroupClassesRaspisId",
                        column: x => x.GroupClassesRaspisId,
                        principalTable: "GroupClassesRaspis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassesGroup_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassesGroup_GroupClassesRaspisId",
                table: "ClassesGroup",
                column: "GroupClassesRaspisId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassesGroup_StudentsId",
                table: "ClassesGroup",
                column: "StudentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassesGroup");
        }
    }
}

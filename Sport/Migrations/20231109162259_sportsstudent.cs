using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Migrations
{
    public partial class sportsstudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Sports_sportsId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "sportsId",
                table: "Students",
                newName: "SportsId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_sportsId",
                table: "Students",
                newName: "IX_Students_SportsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Sports_SportsId",
                table: "Students",
                column: "SportsId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Sports_SportsId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "SportsId",
                table: "Students",
                newName: "sportsId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_SportsId",
                table: "Students",
                newName: "IX_Students_sportsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Sports_sportsId",
                table: "Students",
                column: "sportsId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

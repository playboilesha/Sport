using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Migrations
{
    public partial class adduserinfoandstudentsport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sportsId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_sportsId",
                table: "Students",
                column: "sportsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Sports_sportsId",
                table: "Students",
                column: "sportsId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Sports_sportsId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_sportsId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "sportsId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Migrations
{
    public partial class Coach1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Coach",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coach_UserId",
                table: "Coach",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coach_AspNetUsers_UserId",
                table: "Coach",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coach_AspNetUsers_UserId",
                table: "Coach");

            migrationBuilder.DropIndex(
                name: "IX_Coach_UserId",
                table: "Coach");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Coach");
        }
    }
}

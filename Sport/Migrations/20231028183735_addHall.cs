using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Migrations
{
    public partial class addHall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountPeople = table.Column<int>(type: "int", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SportsId = table.Column<int>(type: "int", nullable: true),
                    KlubId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hall_Klub_KlubId",
                        column: x => x.KlubId,
                        principalTable: "Klub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hall_Sports_SportsId",
                        column: x => x.SportsId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hall_KlubId",
                table: "Hall",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_Hall_SportsId",
                table: "Hall",
                column: "SportsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hall");
        }
    }
}

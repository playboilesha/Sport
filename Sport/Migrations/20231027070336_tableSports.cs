using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Migrations
{
    public partial class tableSports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    About = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameCharacteristic1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutCharacteristic1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameCharacteristic2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutCharacteristic2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameCharacteristic3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutCharacteristic3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sports");
        }
    }
}

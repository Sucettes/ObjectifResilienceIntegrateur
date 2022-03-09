using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwenael.Persistence.Migrations
{
    public partial class Tutoriel_RangeeTutoriel_Categoriev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lienImgBanniere",
                table: "Tutoriels",
                newName: "LienImgBanniere");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LienImgBanniere",
                table: "Tutoriels",
                newName: "lienImgBanniere");
        }
    }
}

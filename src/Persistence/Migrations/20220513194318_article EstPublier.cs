using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwenael.Persistence.Migrations
{
    public partial class articleEstPublier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstPublier",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstPublier",
                table: "Articles");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwenael.Persistence.Migrations
{
    public partial class audio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Poadcasts");

            migrationBuilder.CreateTable(
                name: "Audios",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categorieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    urlImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    urlAudio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audios", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Audios_CategoriesTutos_categorieId",
                        column: x => x.categorieId,
                        principalTable: "CategoriesTutos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audios_categorieId",
                table: "Audios",
                column: "categorieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audios");

            migrationBuilder.CreateTable(
                name: "Poadcasts",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    categorieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    urlAudio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    urlImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poadcasts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Poadcasts_CategoriesTutos_categorieId",
                        column: x => x.categorieId,
                        principalTable: "CategoriesTutos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Poadcasts_categorieId",
                table: "Poadcasts",
                column: "categorieId");
        }
    }
}

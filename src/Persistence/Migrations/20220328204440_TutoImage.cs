using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwenael.Persistence.Migrations
{
    public partial class TutoImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tutoriels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Difficulte = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    Cout = table.Column<double>(type: "float", maxLength: 7, nullable: false),
                    Duree = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    CategorieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstPublier = table.Column<bool>(type: "bit", nullable: false),
                    LienImgBanniere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuteurUserIdId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutoriels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tutoriels_AspNetUsers_AuteurUserIdId",
                        column: x => x.AuteurUserIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tutoriels_Categories_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tutoriels_AuteurUserIdId",
                table: "Tutoriels",
                column: "AuteurUserIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutoriels_CategorieId",
                table: "Tutoriels",
                column: "CategorieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "RangeeTutoriels");

            migrationBuilder.DropTable(
                name: "Tutoriels");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwenael.Persistence.Migrations
{
    public partial class Tutoriel_RangeeTutoriel_Categorie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tutoriels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lienImgBanniere = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Titre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Difficulte = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    Cout = table.Column<decimal>(type: "decimal(18,2)", maxLength: 7, nullable: false),
                    Duree = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    AuteurUserIdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    References = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutoriels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tutoriels_AspNetUsers_AuteurUserIdId",
                        column: x => x.AuteurUserIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TutorielId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Tutoriels_TutorielId",
                        column: x => x.TutorielId,
                        principalTable: "Tutoriels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RangeeTutoriels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LienImg = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ordre = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    TutorielId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangeeTutoriels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RangeeTutoriels_Tutoriels_TutorielId",
                        column: x => x.TutorielId,
                        principalTable: "Tutoriels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TutorielId",
                table: "Categories",
                column: "TutorielId");

            migrationBuilder.CreateIndex(
                name: "IX_RangeeTutoriels_TutorielId",
                table: "RangeeTutoriels",
                column: "TutorielId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutoriels_AuteurUserIdId",
                table: "Tutoriels",
                column: "AuteurUserIdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "RangeeTutoriels");

            migrationBuilder.DropTable(
                name: "Tutoriels");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwenael.Persistence.Migrations
{
    public partial class UpdateProbleBDTuto8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InerText = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesTutos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesTutos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Formations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Idreference = table.Column<int>(type: "int", nullable: false),
                    LinkS3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderInThePage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Poadcasts",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categorie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poadcasts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RangeeTutos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Texte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LienImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TutorielId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangeeTutos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tutos",
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
                    table.PrimaryKey("PK_Tutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tutos_AspNetUsers_AuteurUserIdId",
                        column: x => x.AuteurUserIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tutos_CategoriesTutos_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "CategoriesTutos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tutos_AuteurUserIdId",
                table: "Tutos",
                column: "AuteurUserIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutos_CategorieId",
                table: "Tutos",
                column: "CategorieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Formations");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "Poadcasts");

            migrationBuilder.DropTable(
                name: "RangeeTutos");

            migrationBuilder.DropTable(
                name: "Tutos");

            migrationBuilder.DropTable(
                name: "CategoriesTutos");
        }
    }
}

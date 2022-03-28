using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gwenael.Persistence.Migrations
{
    public partial class AjoutsDesTrucsDeTuto13123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RangeeTutoriels_Tutoriels_TutorielId",
                table: "RangeeTutoriels");

            migrationBuilder.DropIndex(
                name: "IX_RangeeTutoriels_TutorielId",
                table: "RangeeTutoriels");

            migrationBuilder.AlterColumn<Guid>(
                name: "TutorielId",
                table: "RangeeTutoriels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TutorielId",
                table: "RangeeTutoriels",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_RangeeTutoriels_TutorielId",
                table: "RangeeTutoriels",
                column: "TutorielId");

            migrationBuilder.AddForeignKey(
                name: "FK_RangeeTutoriels_Tutoriels_TutorielId",
                table: "RangeeTutoriels",
                column: "TutorielId",
                principalTable: "Tutoriels",
                principalColumn: "Id");
        }
    }
}

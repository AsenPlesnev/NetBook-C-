using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class ChangedGradeTermToEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Term_TermId",
                table: "Grades");

            migrationBuilder.DropTable(
                name: "Term");

            migrationBuilder.DropIndex(
                name: "IX_Grades_TermId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "TermId",
                table: "Grades");

            migrationBuilder.AddColumn<int>(
                name: "Term",
                table: "Grades",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Term",
                table: "Grades");

            migrationBuilder.AddColumn<string>(
                name: "TermId",
                table: "Grades",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Term",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Term", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_TermId",
                table: "Grades",
                column: "TermId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Term_TermId",
                table: "Grades",
                column: "TermId",
                principalTable: "Term",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class AddedNewDbSetsForRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectClassId",
                table: "Grades",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    ClassNumber = table.Column<int>(nullable: false),
                    ClassLetter = table.Column<int>(nullable: false),
                    SchoolYearStart = table.Column<int>(nullable: false),
                    SchoolYearEnd = table.Column<int>(nullable: false),
                    ClassTeacherId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_AspNetUsers_ClassTeacherId",
                        column: x => x.ClassTeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectClasses",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    SubjectId = table.Column<string>(nullable: true),
                    Workload = table.Column<int>(nullable: false),
                    ClassId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectClasses_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectClasses_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SubjectClassId",
                table: "Grades",
                column: "SubjectClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ClassTeacherId",
                table: "Classes",
                column: "ClassTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_IsDeleted",
                table: "Classes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectClasses_ClassId",
                table: "SubjectClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectClasses_IsDeleted",
                table: "SubjectClasses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectClasses_SubjectId",
                table: "SubjectClasses",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_SubjectClasses_SubjectClassId",
                table: "Grades",
                column: "SubjectClassId",
                principalTable: "SubjectClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_SubjectClasses_SubjectClassId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "SubjectClasses");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Students_ClassId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Grades_SubjectClassId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SubjectClassId",
                table: "Grades");
        }
    }
}

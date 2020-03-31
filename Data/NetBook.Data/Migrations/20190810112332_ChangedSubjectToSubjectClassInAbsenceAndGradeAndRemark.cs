using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class ChangedSubjectToSubjectClassInAbsenceAndGradeAndRemark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_Subjects_SubjectId",
                table: "Absences");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_SubjectClasses_SubjectClassId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Subjects_SubjectId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Remarks_Subjects_SubjectId",
                table: "Remarks");

            migrationBuilder.DropIndex(
                name: "IX_Grades_SubjectClassId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "SubjectClassId",
                table: "Grades");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_SubjectClasses_SubjectId",
                table: "Absences",
                column: "SubjectId",
                principalTable: "SubjectClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_SubjectClasses_SubjectId",
                table: "Grades",
                column: "SubjectId",
                principalTable: "SubjectClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Remarks_SubjectClasses_SubjectId",
                table: "Remarks",
                column: "SubjectId",
                principalTable: "SubjectClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_SubjectClasses_SubjectId",
                table: "Absences");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_SubjectClasses_SubjectId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Remarks_SubjectClasses_SubjectId",
                table: "Remarks");

            migrationBuilder.AddColumn<string>(
                name: "SubjectClassId",
                table: "Grades",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SubjectClassId",
                table: "Grades",
                column: "SubjectClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_Subjects_SubjectId",
                table: "Absences",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_SubjectClasses_SubjectClassId",
                table: "Grades",
                column: "SubjectClassId",
                principalTable: "SubjectClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Subjects_SubjectId",
                table: "Grades",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Remarks_Subjects_SubjectId",
                table: "Remarks",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

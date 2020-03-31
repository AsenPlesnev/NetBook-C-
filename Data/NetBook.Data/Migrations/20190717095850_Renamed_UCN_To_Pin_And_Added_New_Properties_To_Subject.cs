using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class RenamedUcnToPinAndAddedNewPropertiesToSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Students_StudentId",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subjects_SubjectId",
                table: "StudentSubject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSubject",
                table: "StudentSubject");

            migrationBuilder.RenameTable(
                name: "StudentSubject",
                newName: "StudentsSubjects");

            migrationBuilder.RenameColumn(
                name: "UCN",
                table: "Students",
                newName: "PIN");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubject_SubjectId",
                table: "StudentsSubjects",
                newName: "IX_StudentsSubjects_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentsSubjects",
                table: "StudentsSubjects",
                columns: new[] { "StudentId", "SubjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsSubjects_Students_StudentId",
                table: "StudentsSubjects",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsSubjects_Subjects_SubjectId",
                table: "StudentsSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsSubjects_Students_StudentId",
                table: "StudentsSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsSubjects_Subjects_SubjectId",
                table: "StudentsSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentsSubjects",
                table: "StudentsSubjects");

            migrationBuilder.RenameTable(
                name: "StudentsSubjects",
                newName: "StudentSubject");

            migrationBuilder.RenameColumn(
                name: "PIN",
                table: "Students",
                newName: "UCN");

            migrationBuilder.RenameIndex(
                name: "IX_StudentsSubjects_SubjectId",
                table: "StudentSubject",
                newName: "IX_StudentSubject_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSubject",
                table: "StudentSubject",
                columns: new[] { "StudentId", "SubjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Students_StudentId",
                table: "StudentSubject",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subjects_SubjectId",
                table: "StudentSubject",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

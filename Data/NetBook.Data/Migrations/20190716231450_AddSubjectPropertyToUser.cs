using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class AddSubjectPropertyToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Subjects");

            migrationBuilder.AddColumn<string>(
                name: "SubjectId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubjectId",
                table: "AspNetUsers",
                column: "SubjectId",
                unique: true,
                filter: "[SubjectId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Subjects_SubjectId",
                table: "AspNetUsers",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Subjects_SubjectId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubjectId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Subjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

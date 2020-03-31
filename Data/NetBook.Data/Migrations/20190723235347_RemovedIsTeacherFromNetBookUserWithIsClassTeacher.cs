using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class RemovedIsTeacherFromNetBookUserWithIsClassTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTeacher",
                table: "AspNetUsers",
                newName: "IsClassTeacher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsClassTeacher",
                table: "AspNetUsers",
                newName: "IsTeacher");
        }
    }
}

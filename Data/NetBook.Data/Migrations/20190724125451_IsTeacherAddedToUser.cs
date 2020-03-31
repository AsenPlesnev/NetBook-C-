using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class IsTeacherAddedToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTeacher",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTeacher",
                table: "AspNetUsers");
        }
    }
}

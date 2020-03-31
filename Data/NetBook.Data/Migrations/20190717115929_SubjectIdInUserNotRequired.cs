using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class SubjectIdInUserNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubjectId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubjectId",
                table: "AspNetUsers",
                column: "SubjectId",
                unique: true,
                filter: "[SubjectId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubjectId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubjectId",
                table: "AspNetUsers",
                column: "SubjectId",
                unique: true);
        }
    }
}

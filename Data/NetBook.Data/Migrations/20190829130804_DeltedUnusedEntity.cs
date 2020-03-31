using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class DeltedUnusedEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_ClassTeacherId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_ClassTeacherId",
                table: "Classes");

            migrationBuilder.AlterColumn<string>(
                name: "ClassTeacherId",
                table: "Classes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ClassTeacherId",
                table: "Classes",
                column: "ClassTeacherId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_ClassTeacherId",
                table: "Classes",
                column: "ClassTeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_ClassTeacherId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_ClassTeacherId",
                table: "Classes");

            migrationBuilder.AlterColumn<string>(
                name: "ClassTeacherId",
                table: "Classes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ClassTeacherId",
                table: "Classes",
                column: "ClassTeacherId",
                unique: true,
                filter: "[ClassTeacherId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_ClassTeacherId",
                table: "Classes",
                column: "ClassTeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

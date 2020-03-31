using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class AddedUserIdToSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Subjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Subjects");
        }
    }
}

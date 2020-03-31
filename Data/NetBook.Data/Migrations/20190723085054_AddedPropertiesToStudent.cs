using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class AddedPropertiesToStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Students",
                newName: "Town");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Students",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Citizenship",
                table: "Students",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Students",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DateOfBirth",
                table: "Students",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Municipality",
                table: "Students",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Students",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Students",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Citizenship",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Municipality",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Town",
                table: "Students",
                newName: "ClassName");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBook.Data.Migrations
{
    public partial class SchoolNowIsDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "School",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "School",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_School_IsDeleted",
                table: "School",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_School_IsDeleted",
                table: "School");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "School");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "School");
        }
    }
}

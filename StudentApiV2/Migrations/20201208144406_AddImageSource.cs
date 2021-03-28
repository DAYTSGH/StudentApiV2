using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApiV2.Migrations
{
    public partial class AddImageSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoursePicture",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Courses");

            migrationBuilder.AddColumn<byte[]>(
                name: "CoursePicture",
                table: "Courses",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}

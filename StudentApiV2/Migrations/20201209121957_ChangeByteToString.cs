using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApiV2.Migrations
{
    public partial class ChangeByteToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Notices");

            migrationBuilder.AddColumn<string>(
                name: "PhotoSource",
                table: "Notices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoSource",
                table: "Notices");

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Notices",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}

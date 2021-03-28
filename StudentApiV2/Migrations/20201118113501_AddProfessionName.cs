using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApiV2.Migrations
{
    public partial class AddProfessionName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfessionName",
                table: "Professions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfessionName",
                table: "Professions");
        }
    }
}

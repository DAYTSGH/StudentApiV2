using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApiV2.Migrations
{
    public partial class Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Scores_CourseId",
                table: "Scores",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_TeacherId",
                table: "Scores",
                column: "TeacherId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Courses_CourseId",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Teachers_TeacherId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_CourseId",
                table: "Scores");
        }
    }
}

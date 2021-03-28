using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApiV2.Migrations
{
    public partial class ChangeScoreIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Teach_Courses_TeacherId_CourseId",
                table: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teach_Courses",
                table: "Teach_Courses");

            migrationBuilder.DropIndex(
                name: "IX_Teach_Courses_TeacherId",
                table: "Teach_Courses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teach_Courses",
                table: "Teach_Courses",
                columns: new[] { "TeacherId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_Teach_Courses_CourseId",
                table: "Teach_Courses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Teach_Courses_TeacherId_CourseId",
                table: "Scores",
                columns: new[] { "TeacherId", "CourseId" },
                principalTable: "Teach_Courses",
                principalColumns: new[] { "TeacherId", "CourseId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Teach_Courses_TeacherId_CourseId",
                table: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teach_Courses",
                table: "Teach_Courses");

            migrationBuilder.DropIndex(
                name: "IX_Teach_Courses_CourseId",
                table: "Teach_Courses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teach_Courses",
                table: "Teach_Courses",
                columns: new[] { "CourseId", "TeacherId" });

            migrationBuilder.CreateIndex(
                name: "IX_Teach_Courses_TeacherId",
                table: "Teach_Courses",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Teach_Courses_TeacherId_CourseId",
                table: "Scores",
                columns: new[] { "TeacherId", "CourseId" },
                principalTable: "Teach_Courses",
                principalColumns: new[] { "CourseId", "TeacherId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}

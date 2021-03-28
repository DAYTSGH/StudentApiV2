using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApiV2.Migrations
{
    public partial class ForeignKeyChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Courses_CourseId",
                table: "Scores");

            migrationBuilder.AddColumn<Guid>(
                name: "Teach_CourseCourseId",
                table: "Scores",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Teach_CourseTeacherId",
                table: "Scores",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_Teach_CourseCourseId_Teach_CourseTeacherId",
                table: "Scores",
                columns: new[] { "Teach_CourseCourseId", "Teach_CourseTeacherId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Teach_Courses_Teach_CourseCourseId_Teach_CourseTeacherId",
                table: "Scores",
                columns: new[] { "Teach_CourseCourseId", "Teach_CourseTeacherId" },
                principalTable: "Teach_Courses",
                principalColumns: new[] { "CourseId", "TeacherId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Teach_Courses_Teach_CourseCourseId_Teach_CourseTeacherId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_Teach_CourseCourseId_Teach_CourseTeacherId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "Teach_CourseCourseId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "Teach_CourseTeacherId",
                table: "Scores");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Courses_CourseId",
                table: "Scores",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

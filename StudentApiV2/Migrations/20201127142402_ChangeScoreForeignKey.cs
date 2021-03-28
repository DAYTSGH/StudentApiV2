using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApiV2.Migrations
{
    public partial class ChangeScoreForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Scores",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_StudentId",
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

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Scores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Scores",
                table: "Scores",
                columns: new[] { "StudentId", "CourseId", "TeacherId" });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_TeacherId_CourseId",
                table: "Scores",
                columns: new[] { "TeacherId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Teach_Courses_TeacherId_CourseId",
                table: "Scores",
                columns: new[] { "TeacherId", "CourseId" },
                principalTable: "Teach_Courses",
                principalColumns: new[] { "CourseId", "TeacherId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Teach_Courses_TeacherId_CourseId",
                table: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Scores",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_TeacherId_CourseId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TeacherId",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Scores",
                table: "Scores",
                columns: new[] { "CourseId", "StudentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_StudentId",
                table: "Scores",
                column: "StudentId");

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
    }
}

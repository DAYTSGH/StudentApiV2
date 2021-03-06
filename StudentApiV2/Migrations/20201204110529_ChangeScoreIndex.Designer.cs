// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentApiV2.Data;

namespace StudentApiV2.Migrations
{
    [DbContext(typeof(ManageDbContext))]
    [Migration("20201204110529_ChangeScoreIndex")]
    partial class ChangeScoreIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("StudentApiV2.Entities.Academy", b =>
                {
                    b.Property<Guid>("AcademyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AcademyCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AcademyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AcademyId");

                    b.ToTable("Academies");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Admin", b =>
                {
                    b.Property<Guid>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdminCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AdminName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AdminPassword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("AdminType")
                        .HasColumnType("int");

                    b.HasKey("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Course", b =>
                {
                    b.Property<Guid>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("CourseCredit")
                        .HasColumnType("float");

                    b.Property<int>("CourseHours")
                        .HasColumnType("int");

                    b.Property<string>("CourseInfo")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("CoursePicture")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("CourseType")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublicshTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Publisher")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Notice", b =>
                {
                    b.Property<Guid>("NoticeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("EditTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Editor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("NoticeId");

                    b.ToTable("Notices");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Profession", b =>
                {
                    b.Property<Guid>("ProfessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AcademyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProfessionCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProfessionName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ProfessionId");

                    b.HasIndex("AcademyId");

                    b.ToTable("Professions");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Score", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ExamScore")
                        .HasColumnType("int");

                    b.Property<double>("FinalScore")
                        .HasColumnType("float");

                    b.Property<int>("MidExamScore")
                        .HasColumnType("int");

                    b.Property<int>("UsualScore")
                        .HasColumnType("int");

                    b.HasKey("StudentId", "CourseId", "TeacherId");

                    b.HasIndex("TeacherId", "CourseId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Student", b =>
                {
                    b.Property<Guid>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProfessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("StudentAge")
                        .HasColumnType("int");

                    b.Property<string>("StudentCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StudentEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentGender")
                        .HasColumnType("int");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StudentPassword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StudentTelephone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentType")
                        .HasColumnType("int");

                    b.HasKey("StudentId");

                    b.HasIndex("ProfessionId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Teach_Course", b =>
                {
                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TeacherId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("Teach_Courses");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Teacher", b =>
                {
                    b.Property<Guid>("TeacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AcademyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TeacherAge")
                        .HasColumnType("int");

                    b.Property<string>("TeacherCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TeacherEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TeacherGender")
                        .HasColumnType("int");

                    b.Property<string>("TeacherName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TeacherPassword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TeacherTelephone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TeacherTitle")
                        .HasColumnType("int");

                    b.Property<int>("TeacherType")
                        .HasColumnType("int");

                    b.HasKey("TeacherId");

                    b.HasIndex("AcademyId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Profession", b =>
                {
                    b.HasOne("StudentApiV2.Entities.Academy", "Academy")
                        .WithMany()
                        .HasForeignKey("AcademyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Academy");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Score", b =>
                {
                    b.HasOne("StudentApiV2.Entities.Student", "Student")
                        .WithMany("Score")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentApiV2.Entities.Teach_Course", "Teach_Course")
                        .WithMany("Score")
                        .HasForeignKey("TeacherId", "CourseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Teach_Course");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Student", b =>
                {
                    b.HasOne("StudentApiV2.Entities.Profession", "Profession")
                        .WithMany()
                        .HasForeignKey("ProfessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profession");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Teach_Course", b =>
                {
                    b.HasOne("StudentApiV2.Entities.Course", "Course")
                        .WithMany("Teach_Course")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentApiV2.Entities.Teacher", "Teacher")
                        .WithMany("Teach_Course")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Teacher", b =>
                {
                    b.HasOne("StudentApiV2.Entities.Academy", "Academy")
                        .WithMany()
                        .HasForeignKey("AcademyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Academy");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Course", b =>
                {
                    b.Navigation("Teach_Course");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Student", b =>
                {
                    b.Navigation("Score");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Teach_Course", b =>
                {
                    b.Navigation("Score");
                });

            modelBuilder.Entity("StudentApiV2.Entities.Teacher", b =>
                {
                    b.Navigation("Teach_Course");
                });
#pragma warning restore 612, 618
        }
    }
}

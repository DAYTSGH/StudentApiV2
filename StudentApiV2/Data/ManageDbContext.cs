using Microsoft.EntityFrameworkCore;
using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Data
{
    public class ManageDbContext:DbContext
    {
        public ManageDbContext(DbContextOptions<ManageDbContext> options) : base(options)
        {
            
        }
        public DbSet<Academy> Academies { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teach_Course> Teach_Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Statuses> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Score的复合主键设置,外键关联
            //Score的CourseId来自Teach_Course的CourseId

            modelBuilder.Entity<Score>().HasKey(x => new { x.StudentId, x.CourseId, x.TeacherId });
            modelBuilder.Entity<Score>().HasOne(x => x.Student).WithMany(p => p.Score).HasForeignKey(p => p.StudentId);
            modelBuilder.Entity<Score>().HasOne(x => x.Teach_Course).WithMany(p => p.Score).HasForeignKey(prop => new { prop.TeacherId,prop.CourseId }).OnDelete(DeleteBehavior.Restrict);
            ////Teach_Course的复合主键设置
            modelBuilder.Entity<Teach_Course>().HasKey(x => new { x.TeacherId, x.CourseId });
            modelBuilder.Entity<Statuses>().HasKey(x => new { x.StudentChoose, x.TeacherGrade });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using StudentApiV2.Data;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Services
{
    public class Teach_CourseRepository : ITeach_CourseRepository
    {
        private readonly ManageDbContext _context;

        public Teach_CourseRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public void AddRelation(Teach_Course teach_Course)
        {
            if(teach_Course == null)
            {
                throw new ArgumentNullException(nameof(teach_Course));
            }
            //判断TeacherId和CourseId是否存在
            //外键依赖自动判断
            _context.Teach_Courses.Add(teach_Course);
        }

        public void DeleteRelation(Teach_Course teach_Course)
        {
            if (teach_Course == null)
            {
                throw new ArgumentNullException(nameof(teach_Course));
            }
            //判断TeacherId和CourseId是否存在
            //外键依赖自动判断
            _context.Teach_Courses.Remove(teach_Course);
        }

        public async Task<IEnumerable<Teach_Course>> GetRelationsForTeacherAsync(Guid teacherId)
        {
            if(teacherId == null)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            //按照课程编号排序
            return await _context.Teach_Courses.
                Where(x => teacherId == x.TeacherId).OrderBy(x => x.CourseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Teach_Course>> GetRelationsForCourseAsync(Guid courseId)
        {
            if (courseId == null)
            {
                throw new ArgumentNullException(nameof(courseId));
            }
            //按照老师编号排序
            return await _context.Teach_Courses.Where(x => courseId == x.CourseId)
                .OrderBy(x => x.TeacherId).ToListAsync();
        }

        public async Task<bool> RelationExistAsync(Guid teacherId, Guid courseId)
        {
            if (teacherId == null)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            if (courseId == null)
            {
                throw new ArgumentNullException(nameof(courseId));
            }
            return await _context.Teach_Courses.AnyAsync(x => x.TeacherId == teacherId && x.CourseId == courseId);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void UpdateRelation(Teach_Course teach_Course)
        {
            //_context.Entry(teach_Course).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Teach_Course>> GetRelationsAsync()
        {
            return await _context.Teach_Courses.ToListAsync();
        }

        public async Task<Teach_Course> GetRelationAsync(Guid teacherId, Guid courseId)
        {
            if(teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            if(courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return await _context.Teach_Courses.FirstOrDefaultAsync(x => x.CourseId == courseId & x.TeacherId == teacherId);
        }

        public Teach_Course GetRelation(Guid teacherId, Guid courseId)

        {
            return _context.Teach_Courses.FirstOrDefault(x => x.CourseId == courseId && x.TeacherId == teacherId);
        }
        //排除一个学生已经选过的课程
        public async Task<IEnumerable<Teach_Course>> GetRelationsForStudentAsync(Guid studentId)
        {
            if(studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }
            //筛选条件？
            var teach_Courses = _context.Teach_Courses;
            //该学生已选的课程
            var scores_Chosen = _context.Scores.Where(x => x.StudentId == studentId);

            var scores_Chosen_Ids = (
                from score_Chosen in scores_Chosen
                select score_Chosen.CourseId
                );
            //teach_Courses中不包含已选课程的
            var teach_Courses_Selectable = (
                from teach_Course in teach_Courses
                where !scores_Chosen_Ids.Contains(teach_Course.CourseId)
                select teach_Course
                );

            return await teach_Courses_Selectable.ToListAsync();
        }
        public bool RelationIsMarked(Guid teacherId, Guid courseId)
        {
            if (teacherId == null)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            if (courseId == null)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            var teach_Course = _context.Teach_Courses.FirstOrDefault(x => x.CourseId == courseId & x.TeacherId == teacherId);

            return teach_Course.IsMarked;
        }
    }
}

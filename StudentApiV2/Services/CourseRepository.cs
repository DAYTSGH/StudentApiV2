using Microsoft.EntityFrameworkCore;
using StudentApiV2.Data;
using StudentApiV2.DtoParameters;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Services
{
    public class CourseRepository:ICourseRepository
    {
        private readonly ManageDbContext _context;
        private readonly ITeach_CourseRepository _teach_CourseRepository;

        public CourseRepository(ManageDbContext context,ITeach_CourseRepository teach_CourseRepository)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            _teach_CourseRepository = teach_CourseRepository ?? throw new ArgumentException(nameof(teach_CourseRepository));
        }
        //查询Course
        public async Task<IEnumerable<Course>> GetCoursesAsync(CourseDtoParameters courseDtoParameters)
        {
            if (courseDtoParameters == null)
            {
                throw new ArgumentNullException(nameof(courseDtoParameters));
            }
            //如果 筛选条件 和 查询字符串 都为空的话
            if (string.IsNullOrWhiteSpace(courseDtoParameters.CourseName) && string.IsNullOrWhiteSpace(courseDtoParameters.SearchTerm))
            {
                return await _context.Courses.ToListAsync();
            }

            var courseItems = _context.Courses as IQueryable<Course>;
            //筛选条件
            if (!string.IsNullOrWhiteSpace(courseDtoParameters.CourseName))
            {
                courseDtoParameters.CourseName = courseDtoParameters.CourseName.Trim();

                courseItems = courseItems.Where(x => x.CourseName == courseDtoParameters.CourseName);
            }
            //查询条件
            if (!string.IsNullOrWhiteSpace(courseDtoParameters.SearchTerm))
            {
                courseDtoParameters.SearchTerm = courseDtoParameters.SearchTerm.Trim();

                courseItems = courseItems.Where(x => x.CourseName.Contains(courseDtoParameters.SearchTerm) ||
                x.CourseCode.Contains(courseDtoParameters.SearchTerm));
            }

            return await courseItems.ToListAsync();
        }
        public async Task<Course> GetCourseAsync(Guid courseId)
        {
            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }
            return await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == courseId);
        }
        public async Task<IEnumerable<Course>> GetCoursesAsync(IEnumerable<Guid> courseIds)
        {
            if (courseIds == null)
            {
                throw new ArgumentNullException(nameof(courseIds));
            }
            return await _context.Courses.
                Where(x => courseIds.Contains(x.CourseId)).
                OrderBy(x => x.CourseId).
                ToListAsync();
        }
        //增删改Course
        public void AddCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            course.CourseId = Guid.NewGuid();

            _context.Courses.Add(course);

        }
        public void UpdateCourse(Course course)
        {
            //_context.Entry(course).State = EntityState.Modified;
        }
        public void DeleteCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            _context.Courses.Remove(course);
        }
        public async Task<bool> CourseExistAsync(Guid courseid)
        {
            if (courseid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseid));
            }
            return await _context.Courses.AnyAsync(x => x.CourseId == courseid);
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
        //排除一个老师已经选过的课程
        public async Task<IEnumerable<Course>> GetCourseForTeacherAsync(Guid teacherId)
        {
            if(teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            var courses = _context.Courses;

            var teach_Courses_Chosen = _context.Teach_Courses.Where(x => x.TeacherId == teacherId);

            var teach_Courses_Chosen_Ids = (
                from teach_Course_Chosen in teach_Courses_Chosen
                select teach_Course_Chosen.CourseId
                );

            var courses_Selectable = (
                from course in courses
                where !teach_Courses_Chosen_Ids.Contains(course.CourseId)
                select course
                );

            return await courses_Selectable.ToListAsync();
        }
    }
}

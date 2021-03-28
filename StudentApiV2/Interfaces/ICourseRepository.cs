
using StudentApiV2.DtoParameters;
using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interface
{
    public interface ICourseRepository
    {
        //查询Course
        Task<IEnumerable<Course>> GetCoursesAsync(CourseDtoParameters courseDtoParameters);
        Task<Course> GetCourseAsync(Guid courseId);
        Task<IEnumerable<Course>> GetCourseForTeacherAsync(Guid teacherId);
        Task<IEnumerable<Course>> GetCoursesAsync(IEnumerable<Guid> courseIds);
        //增删改Course
        void AddCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
        Task<bool> CourseExistAsync(Guid courseId);
        Task<bool> SaveAsync();
    }
}

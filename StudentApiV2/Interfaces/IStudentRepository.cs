using StudentApiV2.DtoParameters;
using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interface
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetStudentsAsync(StudentDtoParameters studentDtoParameters);
        Task<Student> GetStudentAsync(Guid studentId);
        Task<IEnumerable<Student>> GetStudentsAsync(IEnumerable<Guid> studentIds);
        Student GetStudentByCodeAsync(string studentCode);
        //增删改Student
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);

        //修改密码需要单独设置
        void UpdatePassword(Student student);
        Task<bool> StudentExistAsync(Guid studentId);
        Task<bool> SaveAsync();
    }
}

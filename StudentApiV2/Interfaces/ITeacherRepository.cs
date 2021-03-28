
using StudentApi.DtoParameters;
using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interface
{
    public interface ITeacherRepository
    {
       //查询Teacher
        Task<IEnumerable<Teacher>> GetTeachersAsync(TeacherDtoParameters teacherDtoParameters);
        Task<Teacher> GetTeacherAsync(Guid teacherId);
        Teacher GetTeacherByCodeAsync(string teacherCode);
        Task<IEnumerable<Teacher>> GetTeachersAsync(IEnumerable<Guid> teacherIds);
        //增删改Teacher 
        void AddTeacher(Teacher teacher);
        void UpdateTeacher(Teacher teacher);
        void UpdatePassword(Teacher teacher);
        void DeleteTeacher(Teacher teacher);
        Task<bool> TeacherExistAsync(Guid teacherId);
        Task<bool> SaveAsync();
    }
}

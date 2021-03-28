using Microsoft.EntityFrameworkCore;
using StudentApi.DtoParameters;
using StudentApiV2.CodeTable;
using StudentApiV2.Data;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Services
{
    public class TeacherRepository:ITeacherRepository
    {
        private readonly ManageDbContext _context;
        //构造方法注入DbContext对象
        public TeacherRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        //实现接口方法
        public async Task<IEnumerable<Teacher>> GetTeachersAsync(TeacherDtoParameters teacherDtoParameters)
        {
            if (string.IsNullOrWhiteSpace(teacherDtoParameters.TeacherGenderDisplay) && string.IsNullOrWhiteSpace(teacherDtoParameters.SearchTerm))
            {
                return await _context.Teachers.ToListAsync();
            }

            var teacherItems = _context.Teachers as IQueryable<Teacher>;

            if (!string.IsNullOrWhiteSpace(teacherDtoParameters.TeacherGenderDisplay))
            {
                teacherDtoParameters.TeacherGenderDisplay = teacherDtoParameters.TeacherGenderDisplay.Trim();

                var teacherGender = Enum.Parse<Gender>(teacherDtoParameters.TeacherGenderDisplay);

                teacherItems = teacherItems.Where(x => x.TeacherGender == teacherGender);
            }

            if (!string.IsNullOrWhiteSpace(teacherDtoParameters.SearchTerm))
            {
                teacherDtoParameters.SearchTerm = teacherDtoParameters.SearchTerm.Trim();

                teacherItems = teacherItems.Where(x => x.TeacherName.Contains(teacherDtoParameters.SearchTerm));
            }
           
            return await teacherItems.ToListAsync();
        }
        public async Task<Teacher> GetTeacherAsync(Guid teacherId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            return await _context.Teachers.FirstOrDefaultAsync(x => x.TeacherId == teacherId);
        }
        public async Task<IEnumerable<Teacher>> GetTeachersAsync(IEnumerable<Guid> teacherIds)
        {
            if (teacherIds == null)
            {
                throw new ArgumentNullException(nameof(teacherIds));
            }
            return await _context.Teachers.
                Where(x => teacherIds.
                Contains(x.TeacherId)).
                OrderBy(x => x.TeacherName).
                ToListAsync();
        }
        public void AddTeacher(Teacher teacher)
        {
            if (teacher == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }
            teacher.TeacherId = Guid.NewGuid();

            _context.Teachers.Add(teacher);
        }
        public void UpdateTeacher(Teacher teacher)
        {
            //_context.Entry(teacher).State = EntityState.Modified;
        }
        public void UpdatePassword(Teacher teacher)
        {
            //_context.Entry(teacher).State = EntityState.Modified;
        }
        public void DeleteTeacher(Teacher teacher)
        {
            if (teacher == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }
            _context.Teachers.Remove(teacher);
        }
        public async Task<bool> TeacherExistAsync(Guid teacherId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            return await _context.Teachers.AnyAsync(x => x.TeacherId == teacherId);
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public Teacher GetTeacherByCodeAsync(string teacherCode)
        {
            if (string.IsNullOrWhiteSpace(teacherCode))
            {
                throw new ArgumentNullException(nameof(teacherCode));
            }
            return _context.Teachers .FirstOrDefault(x => x.TeacherCode == teacherCode);
        }
    }
}

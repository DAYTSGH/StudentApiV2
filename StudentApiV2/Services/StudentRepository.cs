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
    public class StudentRepository : IStudentRepository
    {
        private readonly ManageDbContext _context;
        public StudentRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        //查询Student
        public async Task<IEnumerable<Student>> GetStudentsAsync(StudentDtoParameters studentDtoParameters)
        {
            if (studentDtoParameters == null)
            {
                throw new ArgumentNullException(nameof(studentDtoParameters));
            }
            //如果 筛选条件 和 查询字符串 都为空的话
            if (string.IsNullOrWhiteSpace(studentDtoParameters.StudentName) && string.IsNullOrWhiteSpace(studentDtoParameters.SearchTerm))
            {
                return await _context.Students.ToListAsync();
            }

            var studentItems = _context.Students as IQueryable<Student>;
            //筛选条件
            if (!string.IsNullOrWhiteSpace(studentDtoParameters.StudentName))
            {
                studentDtoParameters.StudentName = studentDtoParameters.StudentName.Trim();

                studentItems = studentItems.Where(x => x.StudentName == studentDtoParameters.StudentName);
            }
            //查询条件
            if (!string.IsNullOrWhiteSpace(studentDtoParameters.SearchTerm))
            {
                studentDtoParameters.SearchTerm = studentDtoParameters.SearchTerm.Trim();

                studentItems = studentItems.Where(x => x.StudentName.Contains(studentDtoParameters.SearchTerm) ||
                x.StudentCode.Contains(studentDtoParameters.SearchTerm));
            }

            return await studentItems.ToListAsync();
        }
        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }

            return await _context.Students.FirstOrDefaultAsync(x => x.StudentId == studentId);
        }
        public async Task<IEnumerable<Student>> GetStudentsAsync(IEnumerable<Guid> studentIds)
        {
            if (studentIds == null)
            {
                throw new ArgumentNullException(nameof(studentIds));
            }
            return await _context.Students.
                Where(x => studentIds.Contains(x.StudentId)).
                OrderBy(x => x.StudentId).
                ToListAsync();
        }
        //增删改Student
        public void AddStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            student.StudentId = Guid.NewGuid();

            _context.Students.Add(student);

        }
        public void UpdateStudent(Student student)
        {
            //_context.Entry(student).State = EntityState.Modified;
        }
        public void UpdatePassword(Student student)
        {
            //_context.Entry(student).State = EntityState.Modified;
        }

        public void DeleteStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            _context.Students.Remove(student);
        }
        public async Task<bool> StudentExistAsync(Guid studentid)
        {
            if (studentid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentid));
            }
            return await _context.Students.AnyAsync(x => x.StudentId == studentid);
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public Student GetStudentByCodeAsync(string studentCode)
        {
            if (string.IsNullOrWhiteSpace(studentCode))
            {
                throw new ArgumentNullException(nameof(studentCode));
            }
            return _context.Students.FirstOrDefault(x => x.StudentCode == studentCode);
        }
    }
}

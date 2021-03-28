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
    public class ScoreRepository : IScoreRepository
    {
        private readonly ManageDbContext _context;

        public ScoreRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        //查询成绩
        public async Task<Score> GetScoreAsync(Guid studentId, Guid teacherId,Guid courseId)
        {
            if (studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return await _context.Scores.FirstOrDefaultAsync(x => x.StudentId == studentId && x.TeacherId == teacherId && x.CourseId == courseId);
        }

        public async Task<IEnumerable<Score>> GetScoresAsync()
        {
            return await _context.Scores.ToListAsync();
        }
        //某门老师某门课的所有学生的成绩
        public async Task<IEnumerable<Score>> GetScoresForCourseForTeacherAsync(Guid teacherId,Guid courseId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return await _context.Scores.Where(x => x.TeacherId == teacherId && x.CourseId == courseId).ToListAsync();
        }

        public async Task<IEnumerable<Score>> GetScoresForStudentAsync(Guid studentId)
        {
            if(studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }

            return await _context.Scores.Where(x => x.StudentId == studentId).ToListAsync();
        }
        public void AddScore(Score score)
        {
            if (score == null)
            {
                throw new ArgumentNullException(nameof(score));
            }

            _context.Scores.Add(score);
        }

        public void DeleteScore(Score score)
        {
            if (score == null)
            {
                throw new ArgumentNullException(nameof(score));
            }

            _context.Scores.Remove(score);
        }

        public void UpdateScore(Score score)
        {
            //_context.Entry(score).State = EntityState.Modified;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool> ScoreExistAsync(Guid studentId, Guid teacherId, Guid courseId)
        {
            if (studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return await _context.Scores.AnyAsync(x => x.StudentId == studentId && x.TeacherId == teacherId && x.CourseId == courseId);
        }

        public async Task<int> GetStudentCount(Guid teacherId, Guid courseId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }
            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return await _context.Scores.Where(x => x.TeacherId == teacherId && x.CourseId == courseId).CountAsync();
        }
    }
}

using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interface
{
    public interface IScoreRepository
    {
        //所有成绩？（没啥用）
        Task<IEnumerable<Score>> GetScoresAsync();
        //一个学生的所有分数
        Task<IEnumerable<Score>> GetScoresForStudentAsync(Guid studentId);
        //一个老师的某门课程的所有分数（所有学生分数）
        Task<Score> GetScoreAsync(Guid studentId,Guid teacherId,Guid courseId);
        //统计一门课的学生人数？
        Task<int> GetStudentCount(Guid teacherId, Guid courseId);
        Task<IEnumerable<Score>> GetScoresForCourseForTeacherAsync(Guid teacherId,Guid courseId);
        //增删改Score

        //知道课程和学生号才能修改
        void AddScore(Score score);
        void UpdateScore(Score score);
        void DeleteScore(Score score);
        Task<bool> ScoreExistAsync(Guid studentId, Guid teacherId, Guid courseId);
        Task<bool> SaveAsync();
    }
}

using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interface
{
    public interface ITeach_CourseRepository
    {
        Task<IEnumerable<Teach_Course>> GetRelationsAsync();
        //查找老师上过的课程
        Task<IEnumerable<Teach_Course>> GetRelationsForTeacherAsync(Guid teacherId);
        //查找一个课程有多少老师
        Task<IEnumerable<Teach_Course>> GetRelationsForStudentAsync(Guid studentId);
        Task<IEnumerable<Teach_Course>> GetRelationsForCourseAsync(Guid courseId);
        Task<Teach_Course> GetRelationAsync(Guid teacherId, Guid courseId);
        //增删改(关系)
        Teach_Course GetRelation(Guid teacherId, Guid courseId);
        void AddRelation(Teach_Course teach_Course);
        void UpdateRelation(Teach_Course teach_Course);
        void DeleteRelation(Teach_Course teach_Course);
        Task<bool> RelationExistAsync(Guid teacherId,Guid courseId);
        bool RelationIsMarked(Guid teacherId, Guid courseId);
        Task<bool> SaveAsync();
    }
}

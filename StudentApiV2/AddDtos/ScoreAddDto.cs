using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.AddDtos
{
    public class ScoreAddDto
    {
        public int UsualScore { get; set; }
        public int MidExamScore { get; set; }
        public int ExamScore { get; set; }
        public int FinalScore { get; set; }

        //public string StudentName { get; set; }
        public Guid StudentId { get; set; }
        //public Student Student { get; set; }
        //外键和导航属性
        public Guid CourseId { get; set; }
        //public string CourseName { get; set; }
        public Guid TeacherId { get; set; }
        //public string TeacherName { get; set; }
        //public Teach_Course Teach_Course { get; set; }
    }
}

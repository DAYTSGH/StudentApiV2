using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class ScoreDto
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public double CourseCredit { get; set; }
        public int CourseHours { get; set; }
        public string CourseType { get; set; }
        public int UsualScore { get; set; }
        public int MidExamScore { get; set; }
        public int ExamScore { get; set; }
        public int FinalScore { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public bool IsMarked { get; set; }
        public Guid StudentId { get; set; }
        //public Student Student { get; set; }
        //外键和导航属性
        public Guid TeacherId { get; set; }

        public Guid CourseId { get; set; }



        //public Teach_Course Teach_Course { get; set; }
    }
}

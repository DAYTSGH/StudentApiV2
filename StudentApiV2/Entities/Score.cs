using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Entities
{
    public class Score
    {
        public int UsualScore { get; set; }
        public int MidExamScore { get; set; }
        public int ExamScore { get; set; }
        public int FinalScore { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        //外键和导航属性
        public Guid CourseId { get; set; }
        //public Teacher Teacher { get; set; }
        public Guid TeacherId { get; set; }
        //public Course Course { get; set; }
        public Teach_Course Teach_Course { get; set; }
    }
}

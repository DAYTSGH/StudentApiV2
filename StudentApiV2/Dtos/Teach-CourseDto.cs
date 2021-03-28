using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class Teach_CourseDto
    {
        //测试使用
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }

        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public int CourseHours { get; set; }
        public double CourseCredit { get; set; }
        public string CourseType { get; set; }
        //public Course Course { get; set; }
        //public Teacher Teacher { get; set; }
        //public string TeacherCode { get; set; }
        //public Gender TeacherGender { get; set; }
        //public int StudentCount { get; set; }
        public bool IsMarked { get; set; }
        public string AcademyName { get; set; }

    }
}

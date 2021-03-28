using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class TeacherDto
    {
        public Guid TeacherId { get; set; }
        public string TeacherCode { get; set; }
        public string TeacherName { get; set; }
        public string TeacherGender { get; set; }
        //public int TeacherAge { get; set; }
        public string TeacherTitle { get; set; }

        //public string TeacherPassword { get; set; }
        public string TeacherType { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherTelephone { get; set; }

        public string AcademyName { get; set; }
    }
}

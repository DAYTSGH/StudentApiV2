using StudentApiV2.CodeTable;
using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class StudentDto
    {
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string StudentGender { get; set; }
        //public int StudentAge { get; set; }
        public string StudentType { get; set; }
        public string StudentPassword { get; set; }

        public string StudentEmail { get; set; }
        public string StudentTelephone { get; set; }
        //导航属性
        //public Guid ProfessionId { get; set; }
        //public Profession Profession { get; set; }
        public string ProfessionName { get; set; }
        public string AcademyName { get; set; }
    }
}

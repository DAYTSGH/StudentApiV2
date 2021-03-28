using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.UpdateDtos
{
    public class StudentUpdateDto
    {
        //public Guid StudentId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        //public Gender StudentGender { get; set; }
        public int StudentAge { get; set; }
        //public UserType StudentType { get; set; }
        //public string StudentPassword { get; set; }

        public string StudentEmail { get; set; }
        public string StudentTelephone { get; set; }
        //导航属性
        public Guid ProfessionId { get; set; }
        //public Profession Profession { get; set; }
        //public string ProfessionName { get; set; }
        //public string AcademyName { get; set; }
    }
}

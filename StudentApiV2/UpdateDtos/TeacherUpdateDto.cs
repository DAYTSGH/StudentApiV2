using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.UpdateDtos
{
    public class TeacherUpdateDto
    {
        //public Guid TeacherId { get; set; }
        public string TeacherCode { get; set; }
        public string TeacherName { get; set; }
        //public Gender TeacherGender { get; set; }
        public int TeacherAge { get; set; }
        public TeacherTitle TeacherTitle { get; set; }
        //更改密码作为单独接口
        //public string TeacherPassword { get; set; }
        //public UserType TeacherType { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherTelephone { get; set; }
    }
}

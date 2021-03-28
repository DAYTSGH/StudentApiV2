using StudentApiV2.CodeTable;
using System;

namespace StudentApiV2.AddDtos
{
    public class TeacherAddDto
    {
        //public Guid TeacherId { get; set; }
        public string TeacherCode { get; set; }
        public string TeacherName { get; set; }
        public Gender TeacherGender { get; set; }
        public TeacherTitle TeacherTitle { get; set; }
        //public string TeacherPassword { get; set; }
        //public UserType TeacherType { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherTelephone { get; set; }

        //外键和导航属性
        //public Guid AcademyId { get; set; }
        //public Academy Academy { get; set; }

        //public List<Teach_Course> Teach_Course { get; set; }
    }
}
using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Entities
{
    public class Teacher
    {
        [Key]
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        [MaxLength(100)]
        public string TeacherCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string TeacherName { get; set; }
        [Required]
        public Gender TeacherGender { get; set; }
        [Required]
        public TeacherTitle TeacherTitle { get; set; }
        [Required]
        [MaxLength(100)]
        public string TeacherPassword { get; set; } = "e10adc3949ba59abbe56e057f20f883e";
        [Required]
        public UserType TeacherType { get; set; } = UserType.老师;
        public string TeacherEmail { get; set; }
        public string TeacherTelephone { get; set; }

        //外键和导航属性
        public Guid AcademyId { get; set; }
        public Academy Academy { get; set; }

        public List<Teach_Course> Teach_Course { get; set; }
    }
}

using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Entities
{
    public class Student
    {
        [Required]
        [Key]
        public Guid StudentId { get; set; }
        [Required]
        [MaxLength(100)]
        public string StudentCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string StudentName { get; set; }
        [Required]
        public Gender StudentGender { get; set; }
        //public int StudentAge { get; set; }
        [Required]
        public UserType StudentType { get; set; } = UserType.学生;
        [Required]
        [MaxLength(100)]
        public string StudentPassword { get; set; } = "e10adc3949ba59abbe56e057f20f883e";

        public string StudentEmail { get; set; }
        public string StudentTelephone { get; set; }
        //导航属性
        public Guid ProfessionId { get; set; }
        public Profession Profession { get; set; }
        public List<Score> Score { get; set; }
    }
}

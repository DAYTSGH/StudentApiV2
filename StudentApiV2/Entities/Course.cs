using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Entities
{
    public class Course
    {
        [Key]
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        [MaxLength(100)]
        public string CourseCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string CourseName { get; set; }
        [Required]
        public int CourseHours { get; set; }
        [Required]
        public double CourseCredit { get; set; }

        public string ImageSource { get; set; }
        [MaxLength(500)]
        public string CourseInfo { get; set; }
        [Required]
        public CourseType CourseType { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicshTime { get; set; }

        public DateTime AddTime { get; set; }
        //外键和导航属性
        //无法设置学院作为外键，不然会造成重复
        //public Guid AcademyId { get; set; }
        //public Academy Academy { get; set; }
        public List<Teach_Course> Teach_Course { get; set; }
    }
}

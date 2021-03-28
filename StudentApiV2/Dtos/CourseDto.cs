using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class CourseDto
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int CourseHours { get; set; }
        public double CourseCredit { get; set; }
        public string ImageSource { get; set; }
        public string CourseInfo { get; set; }
        public string CourseType { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicshTime { get; set; }
        //不用显示创建时间
        //public DateTime AddTime { get; set; }
    }
}

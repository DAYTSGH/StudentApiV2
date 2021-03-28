using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.UpdateDtos
{
    public class CourseUpdateDto
    {
        //public Guid CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int CourseHours { get; set; }
        public double CourseCredit { get; set; }
        public string CourseInfo { get; set; }
        public CourseType CourseType { get; set; }
        //public string Publisher { get; set; }
        //public DateTime PublicshTime { get; set; }
        //自动更新时间
        //public DateTime AddTime { get; set; }
    }
}

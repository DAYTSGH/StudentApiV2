using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.AddDtos
{
    public class CourseAddDto
    {
        //public Guid CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int CourseHours { get; set; }
        public double CourseCredit { get; set; }
        public string ImageSource { get; set; }
        public string CourseInfo { get; set; }
        public CourseType CourseType { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicshTime { get; set; }
        //自动生成
        //public DateTime AddTime { get; set; }
    }
}

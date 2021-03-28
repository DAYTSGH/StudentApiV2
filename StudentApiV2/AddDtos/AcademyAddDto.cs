using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.AddDtos
{
    public class AcademyAddDto
    {
        //Id可以自行生成
        //public Guid AcademyId { get; set; }
        public string AcademyCode { get; set; }
        public string AcademyName { get; set; }
    }
}

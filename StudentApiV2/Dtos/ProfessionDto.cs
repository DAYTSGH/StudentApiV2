using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class ProfessionDto
    {
        public Guid ProfessionId { get; set; }
        public string ProfessionCode { get; set; }
        public string ProfessionName { get; set; }
        public string AcademyName { get; set; }
        //public Guid AcademyId { get; set; }
        //public Academy Academy { get; set; }
    }
}

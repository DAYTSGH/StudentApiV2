using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class AcademyDto
    {
        public Guid AcademyId { get; set; }
        public string AcademyCode { get; set; }
        public string AcademyName { get; set; }
    }
}

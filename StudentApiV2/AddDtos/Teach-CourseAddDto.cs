using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.AddDtos
{
    public class Teach_CourseAddDto
    {
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }

    }
}

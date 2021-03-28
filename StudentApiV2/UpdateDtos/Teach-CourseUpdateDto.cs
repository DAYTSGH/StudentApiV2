using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.UpdateDtos
{
    public class Teach_CourseUpdateDto
    {
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Entities
{
    //老师选择的课程
    public class Teach_Course
    {
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public List<Score> Score { get; set; }
        public Course Course { get; set; }
        public Teacher Teacher { get; set; }
        public bool IsMarked { get; set; } = false;

    }
}

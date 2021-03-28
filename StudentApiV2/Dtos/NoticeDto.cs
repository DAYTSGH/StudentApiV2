using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class NoticeDto
    {
        public Guid NoticeId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string PhotoSource { get; set; }
        public string EditTime { get; set; }
        public string Editor { get; set; }
    }
}

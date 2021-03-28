using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.UpdateDtos
{
    public class NoticeUpdateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        //public byte[] Photo { get; set; }
        public DateTime EditTime { get; set; }
        public string Editor { get; set; }
    }
}

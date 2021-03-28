using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.AddDtos
{
    public class NoticeAddDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        //图片内容之后再说？
        public string PhotoSource { get; set; }
        //编辑时间自动生成？
        public DateTime EditTime { get; set; }
        public string Editor { get; set; }
    }
}

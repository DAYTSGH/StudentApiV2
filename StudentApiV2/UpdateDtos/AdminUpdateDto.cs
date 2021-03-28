using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.UpdateDtos
{
    public class AdminUpdateDto
    {
        //public Guid AdminId { get; set; }
        public string AdminCode { get; set; }
        public string AdminName { get; set; }
        //public UserType AdminType { get; set; }
        //更改密码作为其他接口
        //public string AdminPassword { get; set; }
    }
}

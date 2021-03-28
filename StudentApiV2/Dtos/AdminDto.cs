using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class AdminDto
    {   public Guid AdminId { get; set; }
        public string AdminCode { get; set; }
        public string AdminName { get; set; }
        public string AdminType { get; set; }
        //public string AdminPassword { get; set; }
    }
}

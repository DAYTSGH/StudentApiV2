using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.UpdateDtos
{
    public class PasswordUpdateDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

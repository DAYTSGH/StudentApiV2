using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class VerifyCodeDto
    {
        public VerifyCodeDto(string image,string code)
        {
            this.Image = image;
            this.Code = code;
        }
        public string Image { get; set; }
        public string Code { get; set; }
    }
}

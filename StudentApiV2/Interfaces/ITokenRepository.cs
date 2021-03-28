using Newtonsoft.Json;
using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interfaces
{
    public class LoginRequestDto
    { 
        [Required]
        [JsonProperty("username")]
        public string UserName { get; set; }
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
        [Required]
        [JsonProperty("usertype")]
        public UserType UserType { get; set; }
        //[Required]
        //public string VerifyCode { get; set; }
    
    }

    public interface ITokenRepository
    {
        bool IsAuthenticated(LoginRequestDto request,out string token);
    }


}

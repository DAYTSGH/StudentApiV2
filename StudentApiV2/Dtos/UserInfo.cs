using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Dtos
{
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserType {get;set;}
        public string Token { get; set; }

        public UserInfo(Guid userId,string userCode, string userName,string userType, string token)
        {
            UserId = userId;
            UserCode = userCode;
            UserName = userName;
            UserType = userType;
            Token = token;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApiV2.CodeTable;
using StudentApiV2.Dtos;
using StudentApiV2.Interface;
using StudentApiV2.Interfaces;
using StudentApiV2.Utils;

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        //private static string codeNum;
        private readonly ITokenRepository _tokenRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;

        public AuthenticationController(ITokenRepository tokenRepository,IAdminRepository adminRepository,ITeacherRepository teacherRepository,IStudentRepository studentRepository)
        {
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }
        
        //首先获取验证码？

        [AllowAnonymous]
        [HttpGet("VerifyCode")]
        public ActionResult<VerifyCodeDto> GetVerifyCode()
        {
            //Response.ContentType = "image/jpeg";
            using (var stream = VerifyCode.Create(out string code))
            {
                //Response.Cookies.Delete("VERIFY_CODE");
                var buffer = stream.ToArray();
                string prefix = "data:image/png;base64,";
                var code64 = Convert.ToBase64String(buffer);
                VerifyCodeDto verify = new VerifyCodeDto(prefix+code64, code);
                // codeNum = code;
                // 将验证码的token放入cookie 
                //HttpContext.Session.SetString("SESSION_CODE", code);
                //Response.Cookies.Append("COOKIES_CODE",code);              
                return Ok(verify);
            }
        }


        [AllowAnonymous]
        [HttpPost(template:"requestToken")]
        public ActionResult<UserInfo> RequestToken([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            //HttpContext.Request.Cookies.TryGetValue("VERIFY_CODE", out string value);
            //string code = HttpContext.Session.GetString("SESSION_CODE");
            
            

            string token;

            if(_tokenRepository.IsAuthenticated(request,out token))
            {
                //Response.Cookies.Delete("VERIFY_CODE");
                //Response.Cookies.Append("TOKEN", token);
                switch (request.UserType)
                {
                    case UserType.学生:
                        var student =  _studentRepository.GetStudentByCodeAsync(request.UserName);
                        UserInfo user_student = new UserInfo(student.StudentId,request.UserName,student.StudentName,request.UserType.ToString(), token);
                        return Ok(user_student);
                    case UserType.老师:
                        var teacher = _teacherRepository.GetTeacherByCodeAsync(request.UserName);
                        UserInfo user_teacher = new UserInfo(teacher.TeacherId,request.UserName, teacher.TeacherName,request.UserType.ToString(), token);
                        return Ok(user_teacher);
                    case UserType.管理员:
                        var admin = _adminRepository.GetAdminByCodeAsync(request.UserName);
                        UserInfo user_admin = new UserInfo(admin.AdminId,request.UserName,admin.AdminName, request.UserType.ToString(), token);
                        return Ok(user_admin);
                    default:
                        return BadRequest("Invalid Request");
                }
            }

            return BadRequest("Invalid Request");
        }    
    }
}

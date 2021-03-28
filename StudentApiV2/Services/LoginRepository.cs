using Microsoft.EntityFrameworkCore;
using StudentApiV2.CodeTable;
using StudentApiV2.Data;
using StudentApiV2.Interfaces;
using StudentApiV2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Services
{
    public class LoginRepository:ILoginRepository
    {
        private readonly ManageDbContext _context;

        public LoginRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool IsValid(LoginRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string password = MD5Helper.MD5Encode(request.Password);

            switch (request.UserType)
            {
                case UserType.学生:
                    return _context.Students.Any(x => x.StudentCode == request.UserName && x.StudentPassword == password);
                case UserType.老师:
                    return _context.Teachers.Any(x => x.TeacherCode == request.UserName && x.TeacherPassword == password);
                case UserType.管理员:
                    return _context.Admins.Any(x => x.AdminCode == request.UserName && x.AdminPassword == password);
                default:
                    return false;
            }
        }
    }
}

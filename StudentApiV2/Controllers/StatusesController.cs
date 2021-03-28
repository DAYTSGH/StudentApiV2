using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApiV2.CodeTable;
using StudentApiV2.Interfaces;

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : BaseController
    {
        private readonly IStatusesRepository _statusesRepository;

        public StatusesController(IStatusesRepository statusesRepository)
        {
            _statusesRepository = statusesRepository ?? throw new ArgumentNullException(nameof(statusesRepository));
        }

        [HttpGet("StudentChoose")]
        public bool GetStudentChoose()
        {
            return Status.StudentChoose;
        }
        [HttpGet("TeacherGrade")]
        public bool GetTeacherGrade()
        {
            return Status.TeacherGrade;
        }

        [HttpPost("StudentChoose")]
        public ActionResult SwitchStudentChoose()
        {
            Status.StudentChoose = !Status.StudentChoose;
            return Ok();
            
        }

        [HttpPost("TeacherGrade")]
        public ActionResult SwitchTeacherGrade()
        {
            Status.TeacherGrade = !Status.TeacherGrade;
            return Ok();
        }
    }
}

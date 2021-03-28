using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentApiV2.AddDtos;
using StudentApiV2.DtoParameters;
using StudentApiV2.Dtos;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using StudentApiV2.Services;
using StudentApiV2.UpdateDtos;
using StudentApiV2.Utils;

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly IProfessionRepository _professionRepository;
        private readonly IAcademyRepository _academyRepository;

        public StudentsController(IMapper mapper, IStudentRepository studentRepository,IProfessionRepository professionRepository,IAcademyRepository academyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _professionRepository = professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
            _academyRepository = academyRepository ?? throw new ArgumentNullException(nameof(academyRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents([FromQuery] StudentDtoParameters studentDtoParameters)
        {
            var students = await _studentRepository.GetStudentsAsync(studentDtoParameters);
            foreach(var student in students)
            {
                student.Profession = await _professionRepository.GetProfessionAsync(student.ProfessionId);
                student.Profession.Academy = await _academyRepository.GetAcademyAsync(student.Profession.AcademyId);
            }
            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);
            return Ok(studentDtos);
        }

        [HttpGet]
        [Route(template: "{studentId}", Name = nameof(GetStudent))]
        public async Task<ActionResult<StudentDto>> GetStudent(Guid studentId)
        {
            var student = await _studentRepository.GetStudentAsync(studentId);
            student.Profession = await _professionRepository.GetProfessionAsync(student.ProfessionId);
            student.Profession.Academy = await _academyRepository.GetAcademyAsync(student.Profession.AcademyId);
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }

        [HttpPost("profession/{professionId}")]
        public async Task<ActionResult<StudentDto>> CreateStudent(Guid professionId,[FromBody] StudentAddDto studentAddDto)
        {
            //ApiController在遇到studentAddDto为空时可以自动返回400错误
            var student = _mapper.Map<Student>(studentAddDto);
            student.ProfessionId = professionId;
            student.Profession = await _professionRepository.GetProfessionAsync(professionId);
            student.Profession.Academy = await _academyRepository.GetAcademyAsync(student.Profession.AcademyId);

            _studentRepository.AddStudent(student);//只是被添加到DbContext里

            await _studentRepository.SaveAsync();

            var studentDto = _mapper.Map<StudentDto>(student);
            return CreatedAtRoute(nameof(GetStudent), new { studentId = student.StudentId }, studentDto);
        }

        [HttpDelete("{studentId}", Name = nameof(DeleteStudent))]
        public async Task<IActionResult> DeleteStudent(Guid studentId)
        {
            var studentEntity = await _studentRepository.GetStudentAsync(studentId);

            if (studentEntity == null)
            {
                return NotFound();
            }

            _studentRepository.DeleteStudent(studentEntity);
            await _studentRepository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{studentId}")]
        public async Task<ActionResult<StudentDto>> UpdateStudent(Guid studentId, StudentUpdateDto studentUpdateDto)
        {
            if (!await _studentRepository.StudentExistAsync(studentId))
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentAsync(studentId);

            if (student == null)
            {
                var studentToAdd = _mapper.Map<Student>(studentUpdateDto);
                studentToAdd.StudentId = studentId;

                _studentRepository.AddStudent(studentToAdd);

                await _studentRepository.SaveAsync();

                var studentDtoNew = _mapper.Map<StudentDto>(studentToAdd);
                return CreatedAtRoute(nameof(GetStudent), new { studentId = student.StudentId }, studentDtoNew);
            }

            _mapper.Map(studentUpdateDto, student);

            _studentRepository.UpdateStudent(student);

            await _studentRepository.SaveAsync();

            var studentDto = _mapper.Map<StudentDto>(student);

            return CreatedAtRoute(nameof(GetStudent), new { studentId = student.StudentId }, studentDto);
        }

        [HttpPut("password/{studentId}")]
        public async Task<ActionResult> UpdateStudentPassword(Guid studentId,PasswordUpdateDto passwordUpdateDto)
        {
            if (!await _studentRepository.StudentExistAsync(studentId))
            {
                return NotFound();
            }
            var student = await _studentRepository.GetStudentAsync(studentId);
            if (student != null)
            {
                if(MD5Helper.MD5Encode(passwordUpdateDto.OldPassword) == student.StudentPassword)
                {
                    //输入密码正确，开始修改原密码
                    student.StudentPassword = MD5Helper.MD5Encode(passwordUpdateDto.NewPassword);

                    _studentRepository.UpdatePassword(student);
                    await _studentRepository.SaveAsync();
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpPatch(template: "{studentId}")]
        public async Task<IActionResult> PartiallyUpdateStudent(Guid studentId, JsonPatchDocument<StudentUpdateDto> patchDocument)
        {
            if (!await _studentRepository.StudentExistAsync(studentId))
            {
                //更新资源的Id不存在时，直接返回或是重新创建？ 
                return NotFound();
            }

            var studentEntity = await _studentRepository.GetStudentAsync(studentId);

            var studentPatchDto = _mapper.Map<StudentUpdateDto>(studentEntity);

            //需要处理验证错误
            patchDocument.ApplyTo(studentPatchDto, ModelState);

            if (!TryValidateModel(studentPatchDto))
            {
                return ValidationProblem(ModelState);
            }
            //剩下的步骤就是put步骤
            _mapper.Map(studentPatchDto, studentEntity);

            _studentRepository.UpdateStudent(studentEntity);

            await _studentRepository.SaveAsync();

            return NotFound();
        }
    }
}

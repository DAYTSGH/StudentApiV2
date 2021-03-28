using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentApi.DtoParameters;
using StudentApiV2.AddDtos;
using StudentApiV2.Dtos;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using StudentApiV2.UpdateDtos;
using StudentApiV2.Utils;

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TeachersController : BaseController
    {
        private readonly IAcademyRepository _academyRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;
        public TeachersController(IAcademyRepository academyRepository, ITeacherRepository teacherRepository, IMapper mapper)
        {
            _academyRepository = academyRepository ?? throw new
                ArgumentNullException(nameof(academyRepository));
            _teacherRepository = teacherRepository ?? throw new
                ArgumentNullException(nameof(teacherRepository));
            _mapper = mapper ?? throw new
                ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachers([FromQuery] TeacherDtoParameters teacherDtoParameters)
        {
            var teachers = await _teacherRepository.GetTeachersAsync(teacherDtoParameters);
            //为所有对象添加导航属性？
            foreach(var teacher in teachers)
            {
                teacher.Academy = await _academyRepository.GetAcademyAsync(teacher.AcademyId);
            }
            var teacherDtos = _mapper.Map<IEnumerable<TeacherDto>>(teachers);
            
            return Ok(teacherDtos);
        }

        [HttpGet(template: "{teacherId}",Name =nameof(GetTeacher))]
        public async Task<ActionResult<TeacherDto>> GetTeacher(Guid teacherId)
        {
            var teacher = await _teacherRepository.GetTeacherAsync(teacherId);
            //添加导航属性
            teacher.Academy = await _academyRepository.GetAcademyAsync(teacher.AcademyId);

            if (teacher == null)
            {
                return NotFound();
            }
            var teacherDto = _mapper.Map<TeacherDto>(teacher);
            return Ok(teacherDto);
        }

        [HttpPost("academy/{academyId}")]
        public async Task<ActionResult<TeacherDto>> CreateTeacher(Guid academyId, TeacherAddDto teacherAddDto)
        {
            if (!await _academyRepository.AcademyExistAsync(academyId))
            {
                return NotFound();
            }

            var teacher = _mapper.Map<Teacher>(teacherAddDto);
            teacher.AcademyId = academyId;
            //添加导航属性？
            teacher.Academy = await _academyRepository.GetAcademyAsync(academyId);

            _teacherRepository.AddTeacher(teacher);

            await _teacherRepository.SaveAsync();

            var teacherDto = _mapper.Map<TeacherDto>(teacher);

            return CreatedAtRoute(nameof(GetTeacher), new
            { teacherId = teacher.TeacherId
            }, teacherDto);
        }

        [HttpDelete("{teacherId}")]
        public async Task<IActionResult> DeleteTeacher(Guid teacherId)
        {
            var teacherEntity = await _teacherRepository.GetTeacherAsync(teacherId);
            if (teacherEntity == null)
            {
                return NotFound();
            }

            _teacherRepository.DeleteTeacher(teacherEntity);
            await _teacherRepository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{teacherId}")]
        public async Task<ActionResult<TeacherDto>> UpdateTeacher(Guid teacherId,TeacherUpdateDto teacherUpdateDto)
        {
            if(!await _teacherRepository.TeacherExistAsync(teacherId))
            {
                //先简单处理
                return NotFound();
            }

            var teacher = await _teacherRepository.GetTeacherAsync(teacherId);

            if(teacher == null)
            {
                //先简单处理
                //找得到的做更新处理
                return NotFound();
            }

            _mapper.Map(teacherUpdateDto, teacher);

            _teacherRepository.UpdateTeacher(teacher);

            await _teacherRepository.SaveAsync();

            var teacherDto = _mapper.Map<TeacherDto>(teacher);

            return CreatedAtRoute(nameof(GetTeacher), new { teacherId = teacher.TeacherId }, teacherDto);
        }
        [HttpPut("password/{teacherId}")]
        public async Task<ActionResult> UpdateStudentPassword(Guid teacherId, PasswordUpdateDto passwordUpdateDto)
        {
            if (!await _teacherRepository.TeacherExistAsync(teacherId))
            {
                return NotFound();
            }
            var teacher = await _teacherRepository.GetTeacherAsync(teacherId);
            if (teacher != null)
            {
                if (MD5Helper.MD5Encode(passwordUpdateDto.OldPassword) == teacher.TeacherPassword)
                {
                    //输入密码正确，开始修改原密码
                    teacher.TeacherPassword = MD5Helper.MD5Encode(passwordUpdateDto.NewPassword);

                    _teacherRepository.UpdatePassword(teacher);
                    await _teacherRepository.SaveAsync();
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpPatch("{teacherId}")]
        public async Task<IActionResult> PartiallyUpdateTeacher(Guid teacherId,JsonPatchDocument<TeacherUpdateDto> patchDocument)
        {
            if(!await _teacherRepository.TeacherExistAsync(teacherId))
            {
                //查找资源不存在，直接返回/重新创建
                return NotFound();
            }

            var teacherEntity = await _teacherRepository.GetTeacherAsync(teacherId);

            var teacherPatchDto = _mapper.Map<TeacherUpdateDto>(teacherEntity);
            //处理验证错误
            patchDocument.ApplyTo(teacherPatchDto, ModelState);

            if (!TryValidateModel(teacherPatchDto))
            {
                return ValidationProblem(ModelState);
            }

            //如果一切正确，执行更改(PUT)操作
            _mapper.Map(teacherPatchDto, teacherEntity);

            _teacherRepository.UpdateTeacher(teacherEntity);

            await _teacherRepository.SaveAsync();

            return NotFound();
        }
    }
}

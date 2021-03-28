using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentApiV2.AddDtos;
using StudentApiV2.Dtos;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using StudentApiV2.UpdateDtos;

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Teach_CoursesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ITeach_CourseRepository _teach_CourseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IAcademyRepository _academyRepository;
        private readonly IScoreRepository _scoreRepository;

        public Teach_CoursesController(IMapper mapper, ITeach_CourseRepository teach_CourseRepository,ITeacherRepository teacherRepository,ICourseRepository courseRepository,IAcademyRepository academyRepository,IScoreRepository scoreRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _teach_CourseRepository = teach_CourseRepository ?? throw new ArgumentNullException(nameof(teach_CourseRepository));
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _academyRepository = academyRepository ?? throw new ArgumentNullException(nameof(academyRepository));
            //用来判断学生选了哪些课程并排除
            _scoreRepository = scoreRepository ?? throw new ArgumentNullException(nameof(scoreRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teach_CourseDto>>> GetRelations()
        {
            var teach_Courses = await _teach_CourseRepository.GetRelationsAsync();
            //添加导航属性
            foreach(var teach_Course in teach_Courses)
            {
                teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(teach_Course.TeacherId);
                teach_Course.Teacher.Academy = await _academyRepository.GetAcademyAsync(teach_Course.Teacher.AcademyId);
                teach_Course.Course = await _courseRepository.GetCourseAsync(teach_Course.CourseId);
            }
            var teach_CourseDtos = _mapper.Map<IEnumerable<Teach_CourseDto>>(teach_Courses);
            return Ok(teach_CourseDtos);
        }
        //判断某学生能选择哪些课
        //所有课程(teach-course)-该学生已选择的课程(score表)
        [HttpGet("forstudent/{studentId}")]
        public async Task<ActionResult<IEnumerable<Teach_CourseDto>>> GetRelationsForStudent(Guid studentId)
        {
            var teach_Courses = await _teach_CourseRepository.GetRelationsForStudentAsync(studentId);
            //添加导航属性
            foreach (var teach_Course in teach_Courses)
            {
                teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(teach_Course.TeacherId);
                teach_Course.Teacher.Academy = await _academyRepository.GetAcademyAsync(teach_Course.Teacher.AcademyId);
                teach_Course.Course = await _courseRepository.GetCourseAsync(teach_Course.CourseId);
            }
            var teach_CourseDtos = _mapper.Map<IEnumerable<Teach_CourseDto>>(teach_Courses);
            return Ok(teach_CourseDtos);
        }
        //
        [HttpGet]
        [Route(template: "forteacher/{teacherId}", Name = nameof(GetRelationsForTeacher))]
        public async Task<ActionResult<IEnumerable<Teach_CourseDto>>> GetRelationsForTeacher(Guid teacherId)
        {
            if(teacherId == null|| !await _teacherRepository.TeacherExistAsync(teacherId))
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            var teach_Courses = await _teach_CourseRepository.GetRelationsForTeacherAsync(teacherId);

            //添加导航属性
            foreach (var teach_Course in teach_Courses)
            {
                teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(teach_Course.TeacherId);
                teach_Course.Teacher.Academy = await _academyRepository.GetAcademyAsync(teach_Course.Teacher.AcademyId);
                teach_Course.Course = await _courseRepository.GetCourseAsync(teach_Course.CourseId);
            }

            if (teach_Courses == null)
            {
                throw new ArgumentNullException(nameof(teach_Courses));
            }
            var teach_CourseDtos = _mapper.Map<IEnumerable<Teach_CourseDto>>(teach_Courses);
            return Ok(teach_CourseDtos);
        }

        [HttpGet(template: "forcourse/{courseId}", Name = nameof(GetRelationsForCourse))]
        public async Task<ActionResult<IEnumerable<Teach_CourseDto>>> GetRelationsForCourse(Guid courseId)
        {
            if (courseId == null || !await _courseRepository.CourseExistAsync(courseId))
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            var teach_Courses = await _teach_CourseRepository.GetRelationsForCourseAsync(courseId);
            //添加导航属性
            foreach (var teach_Course in teach_Courses)
            {
                teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(teach_Course.TeacherId);
                teach_Course.Teacher.Academy = await _academyRepository.GetAcademyAsync(teach_Course.Teacher.AcademyId);
                teach_Course.Course = await _courseRepository.GetCourseAsync(teach_Course.CourseId);
            }

            if (teach_Courses == null)
            {
                throw new ArgumentNullException(nameof(teach_Courses));
            }
            var teach_CourseDtos = _mapper.Map<IEnumerable<Teach_CourseDto>>(teach_Courses);
            return Ok(teach_CourseDtos);
        }

        [HttpGet(template:"{teacherId}/{courseId}",Name =nameof(GetRelation))]
        public async Task<ActionResult<Teach_Course>> GetRelation(Guid teacherId,Guid courseId)
        {
            var teach_Course = await _teach_CourseRepository.GetRelationAsync(teacherId, courseId);

            //添加导航属性
                
            teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(teach_Course.TeacherId); 
            teach_Course.Teacher.Academy = await _academyRepository.GetAcademyAsync(teach_Course.Teacher.AcademyId);
            teach_Course.Course = await _courseRepository.GetCourseAsync(teach_Course.CourseId);

            if (teach_Course == null)
            {
                throw new ArgumentNullException(nameof(teach_Course));
            }
            var teach_CourseDto = _mapper.Map<Teach_CourseDto>(teach_Course);
            return Ok(teach_CourseDto);
        }
        [HttpGet("IsMarked/{teacherId}/{courseId}")]
        public bool IsRealtionMarked(Guid teacherId, Guid courseId)
        {
            return _teach_CourseRepository.RelationIsMarked(teacherId, courseId);
        }

        [HttpPost]
        public async Task<ActionResult<Teach_CourseDto>> CreateRelation([FromBody] Teach_CourseAddDto teach_CourseAddDto)
        {
            //ApiController在遇到teach_CourseAddDto为空时可以自动返回400错误
            var teach_Course = _mapper.Map<Teach_Course>(teach_CourseAddDto);
            //添加导航属性
            teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(teach_Course.TeacherId);
            teach_Course.Course = await _courseRepository.GetCourseAsync(teach_Course.CourseId);
            teach_Course.Teacher.Academy = await _academyRepository.GetAcademyAsync(teach_Course.Teacher.AcademyId);
            _teach_CourseRepository.AddRelation(teach_Course);//只是被添加到DbContext里
            await _teach_CourseRepository.SaveAsync();

            var teach_CourseDto = _mapper.Map<Teach_CourseDto>(teach_Course);
            return CreatedAtRoute(nameof(GetRelation),new{ teach_Course.TeacherId,teach_Course.CourseId}, teach_CourseDto);
        }

        [HttpDelete(template: "{teacherId}/{courseId}",Name =nameof(DeleteRelation))]
        public async Task<IActionResult> DeleteRelation(Guid teacherId,Guid courseId)
        {
            var teach_CourseEntity = await _teach_CourseRepository.GetRelationAsync(teacherId,courseId);

            if (teach_CourseEntity == null)
            {
                return NotFound();
            }

            _teach_CourseRepository.DeleteRelation(teach_CourseEntity);
            await _teach_CourseRepository.SaveAsync();

            return NoContent();
        }

        [HttpPut(template: "{teacherId}/{courseId}")]
        public async Task<ActionResult<Teach_CourseDto>> UpdateRelation(Guid teacherId,Guid courseId)
        {
            if (!await _teach_CourseRepository.RelationExistAsync(teacherId,courseId))
            {
                return NotFound();
            }

            var teach_Course = await _teach_CourseRepository.GetRelationAsync(teacherId,courseId);

            if (teach_Course == null)
            {
                Teach_CourseUpdateDto teach_CourseUpdateDto = new Teach_CourseUpdateDto();

                teach_CourseUpdateDto.TeacherId = teacherId;
                teach_CourseUpdateDto.CourseId = courseId;

                var teach_CourseToAdd = _mapper.Map<Teach_Course>(teach_CourseUpdateDto);
                _teach_CourseRepository.AddRelation(teach_CourseToAdd);

                await _teach_CourseRepository.SaveAsync();

                var teach_CourseDtoNew = _mapper.Map<Teach_CourseDto>(teach_CourseToAdd);
                return CreatedAtRoute(nameof(GetRelation), teach_CourseDtoNew);
            }

            teach_Course.TeacherId = teacherId;
            teach_Course.CourseId = courseId;

            _teach_CourseRepository.UpdateRelation(teach_Course);

            await _teach_CourseRepository.SaveAsync();

            var teach_CourseDto = _mapper.Map<Teach_CourseDto>(teach_Course);

            return CreatedAtRoute(nameof(GetRelation),teach_CourseDto);
        }

        [HttpPut("IsMarked/{teacherId}/{courseId}")]
        public async Task<ActionResult> RelationIsMarked(Guid teacherId, Guid courseId)
        {
            if (!await _teach_CourseRepository.RelationExistAsync(teacherId, courseId))
            {
                return NotFound();
            }
            var teach_Course = await _teach_CourseRepository.GetRelationAsync(teacherId, courseId);
            if (teach_Course == null)
            {
                return BadRequest();
            }
            if(teach_Course.IsMarked == true)
            {
                return BadRequest();
            }
            teach_Course.TeacherId = teacherId;
            teach_Course.CourseId = courseId;

            teach_Course.IsMarked = true;

            _teach_CourseRepository.UpdateRelation(teach_Course);

            await _teach_CourseRepository.SaveAsync();

            var teach_CourseDto = _mapper.Map<Teach_CourseDto>(teach_Course);

            return Ok();
        }
    }
}

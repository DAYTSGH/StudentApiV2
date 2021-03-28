using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ICourseRepository _courseRepository;
        //private readonly ITeach_CourseRepository _teach_CourseRepository;

        //用来判断老师选了哪些课程的

        public CoursesController(IMapper mapper, ICourseRepository courseRepository,ITeach_CourseRepository teach_CourseRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            //_teach_CourseRepository = teach_CourseRepository ?? throw new ArgumentNullException(nameof(teach_CourseRepository))
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses([FromQuery] CourseDtoParameters courseDtoParameters)
        {
            var courses = await _courseRepository.GetCoursesAsync(courseDtoParameters);
            
            var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);
            return Ok(courseDtos);
        }

        [HttpGet]
        [Route(template: "{courseId}", Name = nameof(GetCourse))]
        public async Task<ActionResult<CourseDto>> GetCourse(Guid courseId)
        {
            var course = await _courseRepository.GetCourseAsync(courseId);
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            var courseDto = _mapper.Map<CourseDto>(course);
            return Ok(courseDto);
        }
        [HttpGet("forteacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesForTeacher(Guid teacherId)
        {
            var courses = await _courseRepository.GetCourseForTeacherAsync(teacherId);

            //添加导航属性
            var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);
            return Ok(courseDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CourseAddDto courseAddDto)
        {
            //ApiController在遇到courseAddDto为空时可以自动返回400错误
            var course = _mapper.Map<Course>(courseAddDto);
            _courseRepository.AddCourse(course);//只是被添加到DbContext里
            await _courseRepository.SaveAsync();

            var courseDto = _mapper.Map<CourseDto>(course);
            return CreatedAtRoute(nameof(GetCourse), new { courseId = course.CourseId }, courseDto);
        }

        [HttpDelete("{courseId}", Name = nameof(DeleteCourse))]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var courseEntity = await _courseRepository.GetCourseAsync(courseId);

            if (courseEntity == null)
            {
                return NotFound();
            }

            _courseRepository.DeleteCourse(courseEntity);
            await _courseRepository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{courseId}")]
        public async Task<ActionResult<CourseDto>> UpdateCourse(Guid courseId, CourseUpdateDto courseUpdateDto)
        {
            if (!await _courseRepository.CourseExistAsync(courseId))
            {
                return NotFound();
            }

            var course = await _courseRepository.GetCourseAsync(courseId);

            if (course == null)
            {
                var courseToAdd = _mapper.Map<Course>(courseUpdateDto);
                courseToAdd.CourseId = courseId;

                _courseRepository.AddCourse(courseToAdd);

                await _courseRepository.SaveAsync();

                var courseDtoNew = _mapper.Map<CourseDto>(courseToAdd);
                return CreatedAtRoute(nameof(GetCourse), new { courseId = course.CourseId }, courseDtoNew);
            }

            _mapper.Map(courseUpdateDto, course);

            _courseRepository.UpdateCourse(course);

            await _courseRepository.SaveAsync();

            var courseDto = _mapper.Map<CourseDto>(course);

            return CreatedAtRoute(nameof(GetCourse), new { courseId = course.CourseId }, courseDto);
        }

        [HttpPatch(template: "{courseId}")]
        public async Task<IActionResult> PartiallyUpdateCourse(Guid courseId, JsonPatchDocument<CourseUpdateDto> patchDocument)
        {
            if (!await _courseRepository.CourseExistAsync(courseId))
            {
                //更新资源的Id不存在时，直接返回或是重新创建？ 
                return NotFound();
            }

            var courseEntity = await _courseRepository.GetCourseAsync(courseId);

            var coursePatchDto = _mapper.Map<CourseUpdateDto>(courseEntity);

            //需要处理验证错误
            patchDocument.ApplyTo(coursePatchDto, ModelState);

            if (!TryValidateModel(coursePatchDto))
            {
                return ValidationProblem(ModelState);
            }
            //剩下的步骤就是put步骤
            _mapper.Map(coursePatchDto, courseEntity);

            _courseRepository.UpdateCourse(courseEntity);

            await _courseRepository.SaveAsync();

            return NotFound();
        }
    }
}

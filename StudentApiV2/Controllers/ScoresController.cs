using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
    public class ScoresController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IScoreRepository _scoreRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeach_CourseRepository _teach_CourseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ICourseRepository _courseRepository;

        public ScoresController(IMapper mapper, IScoreRepository scoreRepository, IStudentRepository studentRepository,ITeach_CourseRepository teach_CourseRepository,ITeacherRepository teacherRepository,ICourseRepository courseRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _scoreRepository = scoreRepository ?? throw new ArgumentNullException(nameof(scoreRepository));
           
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));

            _teach_CourseRepository = teach_CourseRepository ?? throw new ArgumentNullException(nameof(teach_CourseRepository));
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScoreDto>>> GetScores()
        {
            var scores = await _scoreRepository.GetScoresAsync();
            //添加导航属性
            //有问题，很烦
            foreach(var score in scores)
            {
                score.Student = await _studentRepository.GetStudentAsync(score.StudentId);
                score.Teach_Course = await _teach_CourseRepository.GetRelationAsync(score.TeacherId, score.CourseId);
                score.Teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(score.TeacherId);
                score.Teach_Course.Course = await _courseRepository.GetCourseAsync(score.CourseId);
            }

            var scoreDtos = _mapper.Map<IEnumerable<ScoreDto>>(scores);
            return Ok(scoreDtos);
        }

        [HttpGet]
        [Route(template: "{teacherId}/{courseId}/{studentId}", Name = nameof(GetScore))]
        public async Task<ActionResult<ScoreDto>> GetScore(Guid studentId, Guid teacherId, Guid courseId)
        {
            var score = await _scoreRepository.GetScoreAsync(studentId,teacherId,courseId);
            if (score == null)
            {
                throw new ArgumentNullException(nameof(score));
            }
            //导航属性
            score.Student = await _studentRepository.GetStudentAsync(score.StudentId);
            score.Teach_Course = await _teach_CourseRepository.GetRelationAsync(score.TeacherId, score.CourseId);
            score.Teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(score.TeacherId);
            score.Teach_Course.Course = await _courseRepository.GetCourseAsync(score.CourseId);

            var scoreDto = _mapper.Map<ScoreDto>(score);
            return Ok(scoreDto);
        }

        [HttpGet("teacher/{teacherId}/course/{courseId}")]
        public async Task<ActionResult<IEnumerable<ScoreDto>>> GetScoresForCourseForTeacher(Guid teacherId,Guid courseId)
        {
            var scores = await _scoreRepository.GetScoresForCourseForTeacherAsync(teacherId, courseId);
            if (scores == null)
            {
                throw new ArgumentNullException(nameof(scores));
            }
            foreach (var score in scores)
            {
                score.Student = await _studentRepository.GetStudentAsync(score.StudentId);
                score.Teach_Course = await _teach_CourseRepository.GetRelationAsync(score.TeacherId, score.CourseId);
                score.Teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(score.TeacherId);
                score.Teach_Course.Course = await _courseRepository.GetCourseAsync(score.CourseId);
            }
            var scoreDtos = _mapper.Map<IEnumerable<ScoreDto>>(scores);
            return Ok(scoreDtos);
        }

        [HttpGet("studentcount/{teacherId}/{courseId}")]
        public async Task<ActionResult<int>> GetStudentCount(Guid teacherId,Guid courseId)
        {
            return await _scoreRepository.GetStudentCount(teacherId, courseId);
        }

        [HttpGet("forstudent/{studentId}")]
        public async Task<ActionResult<IEnumerable<ScoreDto>>> GetScoresForStudent(Guid studentId)
        {
            var scores = await _scoreRepository.GetScoresForStudentAsync(studentId);
            foreach (var score in scores)
            {
                score.Student = await _studentRepository.GetStudentAsync(score.StudentId);
                score.Teach_Course = await _teach_CourseRepository.GetRelationAsync(score.TeacherId, score.CourseId);
                score.Teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(score.TeacherId);
                score.Teach_Course.Course = await _courseRepository.GetCourseAsync(score.CourseId);
            }

            var scoreDtos = _mapper.Map<IEnumerable<ScoreDto>>(scores);
            return Ok(scoreDtos);
        }
        [HttpPost]
        public async Task<ActionResult<ScoreDto>> CreateScore([FromBody] ScoreAddDto scoreAddDto)
        {
            if (!await _studentRepository.StudentExistAsync(scoreAddDto.StudentId))
            {
                return NotFound();
            }
            if (!await _teach_CourseRepository.RelationExistAsync(scoreAddDto.TeacherId, scoreAddDto.CourseId))
            {
                return NotFound();
            }

            //ApiController在遇到scoreAddDto为空时可以自动返回400错误
            var score = _mapper.Map<Score>(scoreAddDto);

            score.StudentId = scoreAddDto.StudentId;
            score.Student = await _studentRepository.GetStudentAsync(scoreAddDto.StudentId);

            score.TeacherId = scoreAddDto.TeacherId;
            score.CourseId = scoreAddDto.CourseId;

            score.Teach_Course = await _teach_CourseRepository.GetRelationAsync(scoreAddDto.TeacherId, scoreAddDto.CourseId);

            _scoreRepository.AddScore(score);//只是被添加到DbContext里
            
            await _scoreRepository.SaveAsync();

            var scoreDto = _mapper.Map<ScoreDto>(score);
            return CreatedAtRoute(nameof(GetScore), new { studentId = score.StudentId,teacherId = score.TeacherId,courseId = score.CourseId }, scoreDto);
        }

        [HttpDelete("{teacherId}/{courseId}/{studentId}", Name = nameof(DeleteScore))]
        public async Task<IActionResult> DeleteScore(Guid studentId, Guid teacherId, Guid courseId)
        {
            var scoreEntity = await _scoreRepository.GetScoreAsync(studentId, teacherId, courseId);

            if (scoreEntity == null)
            {
                return NotFound();
            }

            _scoreRepository.DeleteScore(scoreEntity);
            await _scoreRepository.SaveAsync();

            return NoContent();
        }

        [HttpPut(template: "{teacherId}/{courseId}/{studentId}")]
        public async Task<ActionResult<ScoreDto>> UpdateScore(Guid studentId, Guid teacherId, Guid courseId,ScoreUpdateDto scoreUpdateDto)
        {
            if (!await _scoreRepository.ScoreExistAsync(studentId, teacherId, courseId))
            {
                return NotFound();
            }

            var score = await _scoreRepository.GetScoreAsync(studentId, teacherId, courseId);

            if (score == null)
            {
                var scoreToAdd = _mapper.Map<Score>(scoreUpdateDto);
                scoreToAdd.StudentId = studentId;
                scoreToAdd.TeacherId = teacherId;
                scoreToAdd.CourseId = courseId;

                _scoreRepository.AddScore(scoreToAdd);

                await _scoreRepository.SaveAsync();
                //添加导航属性？
                scoreToAdd.Student = await _studentRepository.GetStudentAsync(studentId);
                scoreToAdd.Teach_Course = await _teach_CourseRepository.GetRelationAsync(teacherId, courseId);
                scoreToAdd.Teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(teacherId);
                scoreToAdd.Teach_Course.Course = await _courseRepository.GetCourseAsync(courseId);

                var scoreDtoNew = _mapper.Map<ScoreDto>(scoreToAdd);

                

                return CreatedAtRoute(nameof(GetScore), new { studentId = scoreToAdd.StudentId, teacherId = scoreToAdd.TeacherId, courseId = scoreToAdd.CourseId }, scoreDtoNew);
            }

            _mapper.Map(scoreUpdateDto, score);

            _scoreRepository.UpdateScore(score);

            await _scoreRepository.SaveAsync();

            score.Student = await _studentRepository.GetStudentAsync(studentId);
            score.Teach_Course = await _teach_CourseRepository.GetRelationAsync(teacherId, courseId);
            score.Teach_Course.Teacher = await _teacherRepository.GetTeacherAsync(teacherId);
            score.Teach_Course.Course = await _courseRepository.GetCourseAsync(courseId);

            var scoreDto = _mapper.Map<ScoreDto>(score);

            

            return CreatedAtRoute(nameof(GetScore), new { studentId = score.StudentId, teacherId = score.TeacherId, courseId = score.CourseId }, scoreDto);
        }

        [HttpPatch(template: "{teacherId}/{courseId}/{studentId}")]
        public async Task<IActionResult> PartiallyUpdateScore(Guid studentId, Guid teacherId, Guid courseId, JsonPatchDocument<ScoreUpdateDto> patchDocument)
        {
            if (!await _scoreRepository.ScoreExistAsync(studentId, teacherId, courseId))
            {
                //更新资源的Id不存在时，直接返回或是重新创建？ 
                return NotFound();
            }

            var scoreEntity = await _scoreRepository.GetScoreAsync(studentId, teacherId, courseId);

            var scorePatchDto = _mapper.Map<ScoreUpdateDto>(scoreEntity);

            //需要处理验证错误
            patchDocument.ApplyTo(scorePatchDto, ModelState);

            if (!TryValidateModel(scorePatchDto))
            {
                return ValidationProblem(ModelState);
            }
            //剩下的步骤就是put步骤
            _mapper.Map(scorePatchDto, scoreEntity);

            _scoreRepository.UpdateScore(scoreEntity);

            await _scoreRepository.SaveAsync();

            return NotFound();
        }
    }
}

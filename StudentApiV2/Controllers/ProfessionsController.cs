using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentApiV2.AddDtos;
using StudentApiV2.DtoParameters;
using StudentApiV2.Dtos;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using StudentApiV2.UpdateDtos;

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProfessionRepository _professionRepository;
        private readonly IAcademyRepository _academyRepository;

        public ProfessionsController(IMapper mapper, IProfessionRepository professionRepository,IAcademyRepository academyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _professionRepository = professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
            _academyRepository = academyRepository ?? throw new ArgumentNullException(nameof(academyRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfessionDto>>> GetProfessions([FromQuery] ProfessionDtoParameters professionDtoParameters)
        {
            var professions = await _professionRepository.GetProfessionsAsync(professionDtoParameters);
            //添加导航属性
            foreach(var professin in professions)
            {
                professin.Academy = await _academyRepository.GetAcademyAsync(professin.AcademyId);
            }
            var professionDtos = _mapper.Map<IEnumerable<ProfessionDto>>(professions);
            return Ok(professionDtos);
        }
        [HttpGet("academy/{academyId}")]
        public async Task<ActionResult<IEnumerable<ProfessionDto>>> GetProfessionsForAcademy(Guid academyId)
        {
            var professions = await _professionRepository.GetProfessionForAcademyAsync(academyId);
            foreach (var professin in professions)
            {
                professin.Academy = await _academyRepository.GetAcademyAsync(professin.AcademyId);
            }
            var professionDtos = _mapper.Map<IEnumerable<ProfessionDto>>(professions);
            return Ok(professionDtos);
        }

        [HttpGet]
        [Route(template: "{professionId}", Name = nameof(GetProfession))]
        public async Task<ActionResult<ProfessionDto>> GetProfession(Guid professionId)
        {
            var profession = await _professionRepository.GetProfessionAsync(professionId);
            profession.Academy = await _academyRepository.GetAcademyAsync(profession.AcademyId);
            if (profession == null)
            {
                throw new ArgumentNullException(nameof(profession));
            }
            var professionDto = _mapper.Map<ProfessionDto>(profession);
            return Ok(professionDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProfessionDto>> CreateProfession(Guid academyId,[FromBody] ProfessionAddDto professionAddDto)
        {
            if(!await _academyRepository.AcademyExistAsync(academyId))
            {
                return NotFound();
            }
            //ApiController在遇到professionAddDto为空时可以自动返回400错误
            var profession = _mapper.Map<Profession>(professionAddDto);

            profession.AcademyId = academyId;
            //添加导航属性
            profession.Academy = await _academyRepository.GetAcademyAsync(academyId);

            _professionRepository.AddProfession(profession);//只是被添加到DbContext里
            await _professionRepository.SaveAsync();

            var professionDto = _mapper.Map<ProfessionDto>(profession);
            return CreatedAtRoute(nameof(GetProfession), new { professionId = profession.ProfessionId }, professionDto);
        }

        [HttpDelete("{professionId}", Name = nameof(DeleteProfession))]
        public async Task<IActionResult> DeleteProfession(Guid professionId)
        {
            var professionEntity = await _professionRepository.GetProfessionAsync(professionId);

            if (professionEntity == null)
            {
                return NotFound();
            }

            _professionRepository.DeleteProfession(professionEntity);
            await _professionRepository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{professionId}")]
        public async Task<ActionResult<ProfessionDto>> UpdateProfession(Guid professionId, ProfessionUpdateDto professionUpdateDto)
        {
            if (!await _professionRepository.ProfessionExistAsync(professionId))
            {
                return NotFound();
            }

            var profession = await _professionRepository.GetProfessionAsync(professionId);

            if (profession == null)
            {
                var professionToAdd = _mapper.Map<Profession>(professionUpdateDto);
                professionToAdd.ProfessionId = professionId;

                _professionRepository.AddProfession(professionToAdd);

                await _professionRepository.SaveAsync();

                var professionDtoNew = _mapper.Map<ProfessionDto>(professionToAdd);
                return CreatedAtRoute(nameof(GetProfession), new { professionId = profession.ProfessionId }, professionDtoNew);
            }

            _mapper.Map(professionUpdateDto, profession);

            _professionRepository.UpdateProfession(profession);

            await _professionRepository.SaveAsync();

            var professionDto = _mapper.Map<ProfessionDto>(profession);

            return CreatedAtRoute(nameof(GetProfession), new { professionId = profession.ProfessionId }, professionDto);
        }

        [HttpPatch(template: "{professionId}")]
        public async Task<IActionResult> PartiallyUpdateProfession(Guid professionId, JsonPatchDocument<ProfessionUpdateDto> patchDocument)
        {
            if (!await _professionRepository.ProfessionExistAsync(professionId))
            {
                //更新资源的Id不存在时，直接返回或是重新创建？ 
                return NotFound();
            }

            var professionEntity = await _professionRepository.GetProfessionAsync(professionId);

            var professionPatchDto = _mapper.Map<ProfessionUpdateDto>(professionEntity);

            //需要处理验证错误
            patchDocument.ApplyTo(professionPatchDto, ModelState);

            if (!TryValidateModel(professionPatchDto))
            {
                return ValidationProblem(ModelState);
            }
            //剩下的步骤就是put步骤
            _mapper.Map(professionPatchDto, professionEntity);

            _professionRepository.UpdateProfession(professionEntity);

            await _professionRepository.SaveAsync();

            return NotFound();
        }
    }
}

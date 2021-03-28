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
using StudentApiV2.Services;
using StudentApiV2.UpdateDtos;

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademiesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IAcademyRepository _academyRepository;
        public AcademiesController(IMapper mapper,IAcademyRepository academyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _academyRepository = academyRepository ?? throw new ArgumentNullException(nameof(academyRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AcademyDto>>> GetAcademies([FromQuery] AcademyDtoParameters academyDtoParameters)
        {
            var academies = await _academyRepository.GetAcademiesAsync(academyDtoParameters);
            var academyDtos = _mapper.Map<IEnumerable<AcademyDto>>(academies);
            return Ok(academyDtos);
        }

        [HttpGet]
        [Route(template: "{academyId}", Name = nameof(GetAcademy))]
        public async Task<ActionResult<AcademyDto>> GetAcademy(Guid academyId)
        {
            var academy = await _academyRepository.GetAcademyAsync(academyId);
            if (academy == null)
            {
                throw new ArgumentNullException(nameof(academy));
            }
            var academyDto = _mapper.Map<AcademyDto>(academy);
            return Ok(academyDto);
        }

        [HttpPost]
        public async Task<ActionResult<AcademyDto>> CreateAcademy([FromBody] AcademyAddDto academyAddDto)
        {
            //ApiController在遇到academyAddDto为空时可以自动返回400错误
            var academy = _mapper.Map<Academy>(academyAddDto);
            _academyRepository.AddAcademy(academy);//只是被添加到DbContext里
            await _academyRepository.SaveAsync();

            var academyDto = _mapper.Map<AcademyDto>(academy);
            return CreatedAtRoute(nameof(GetAcademy), new { academyId = academy.AcademyId }, academyDto);
        }

        [HttpDelete("{academyId}", Name = nameof(DeleteAcademy))]
        public async Task<IActionResult> DeleteAcademy(Guid academyId)
        {
            var academyEntity = await _academyRepository.GetAcademyAsync(academyId);

            if (academyEntity == null)
            {
                return NotFound();
            }

            _academyRepository.DeleteAcademy(academyEntity);
            await _academyRepository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{academyId}")]
        public async Task<ActionResult<AcademyDto>> UpdateAcademy(Guid academyId,AcademyUpdateDto academyUpdateDto)
        {
            if(!await _academyRepository.AcademyExistAsync(academyId))
            {
                return NotFound();
            }

            var academy = await _academyRepository.GetAcademyAsync(academyId);

            if (academy == null)
            {
                var academyToAdd = _mapper.Map<Academy>(academyUpdateDto);
                academyToAdd.AcademyId = academyId;

                _academyRepository.AddAcademy(academyToAdd);

                await _academyRepository.SaveAsync();

                var academyDtoNew = _mapper.Map<AcademyDto>(academyToAdd);
                return CreatedAtRoute(nameof(GetAcademy), new { academyId = academy.AcademyId }, academyDtoNew);
            }

            _mapper.Map(academyUpdateDto, academy);

            _academyRepository.UpdateAcademy(academy);

            await _academyRepository.SaveAsync();

            var academyDto = _mapper.Map<AcademyDto>(academy);

            return CreatedAtRoute(nameof(GetAcademy), new { academyId = academy.AcademyId }, academyDto);
        }

        [HttpPatch(template:"{academyId}")]
        public async Task<IActionResult> PartiallyUpdateAcademy(Guid academyId,JsonPatchDocument<AcademyUpdateDto> patchDocument)
        {
            if(!await _academyRepository.AcademyExistAsync(academyId))
            {
                //更新资源的Id不存在时，直接返回或是重新创建？ 
                return NotFound();
            }

            var academyEntity = await _academyRepository.GetAcademyAsync(academyId);

            var academyPatchDto = _mapper.Map<AcademyUpdateDto>(academyEntity);

            //需要处理验证错误
            patchDocument.ApplyTo(academyPatchDto,ModelState);

            if (!TryValidateModel(academyPatchDto))
            {
                return ValidationProblem(ModelState);
            }
            //剩下的步骤就是put步骤
            _mapper.Map(academyPatchDto, academyEntity);

            _academyRepository.UpdateAcademy(academyEntity);

            await _academyRepository.SaveAsync();

            return NotFound();
        }
    }
}

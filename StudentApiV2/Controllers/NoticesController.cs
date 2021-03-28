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
using StudentApiV2.UpdateDtos;

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly INoticeRepository _noticeRepository;
        public NoticesController(IMapper mapper, INoticeRepository noticeRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _noticeRepository = noticeRepository ?? throw new ArgumentNullException(nameof(noticeRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoticeDto>>> GetNotices([FromQuery] NoticeDtoParameters noticeDtoParameters)
        {
            var notices = await _noticeRepository.GetNoticesAsync(noticeDtoParameters);
            var noticeDtos = _mapper.Map<IEnumerable<NoticeDto>>(notices);
            return Ok(noticeDtos);
        }

        [HttpGet]
        [Route(template: "{noticeId}", Name = nameof(GetNotice))]
        public async Task<ActionResult<NoticeDto>> GetNotice(Guid noticeId)
        {
            var notice = await _noticeRepository.GetNoticeAsync(noticeId);
            if (notice == null)
            {
                throw new ArgumentNullException(nameof(notice));
            }
            var noticeDto = _mapper.Map<NoticeDto>(notice);
            return Ok(noticeDto);
        }

        [HttpPost]
        public async Task<ActionResult<NoticeDto>> CreateNotice([FromBody] NoticeAddDto noticeAddDto)
        {
            //ApiController在遇到noticeAddDto为空时可以自动返回400错误
            var notice = _mapper.Map<Notice>(noticeAddDto);
            _noticeRepository.AddNotice(notice);//只是被添加到DbContext里
            await _noticeRepository.SaveAsync();

            var noticeDto = _mapper.Map<NoticeDto>(notice);
            return CreatedAtRoute(nameof(GetNotice), new { noticeId = notice.NoticeId }, noticeDto);
        }

        [HttpDelete("{noticeId}", Name = nameof(DeleteNotice))]
        public async Task<IActionResult> DeleteNotice(Guid noticeId)
        {
            var noticeEntity = await _noticeRepository.GetNoticeAsync(noticeId);

            if (noticeEntity == null)
            {
                return NotFound();
            }

            _noticeRepository.DeleteNotice(noticeEntity);
            await _noticeRepository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{noticeId}")]
        public async Task<ActionResult<NoticeDto>> UpdateNotice(Guid noticeId, NoticeUpdateDto noticeUpdateDto)
        {
            if (!await _noticeRepository.NoticeExistAsync(noticeId))
            {
                return NotFound();
            }

            var notice = await _noticeRepository.GetNoticeAsync(noticeId);

            if (notice == null)
            {
                var noticeToAdd = _mapper.Map<Notice>(noticeUpdateDto);
                noticeToAdd.NoticeId = noticeId;

                _noticeRepository.AddNotice(noticeToAdd);

                await _noticeRepository.SaveAsync();

                var noticeDtoNew = _mapper.Map<NoticeDto>(noticeToAdd);
                return CreatedAtRoute(nameof(GetNotice), new { noticeId = notice.NoticeId }, noticeDtoNew);
            }

            _mapper.Map(noticeUpdateDto, notice);

            _noticeRepository.UpdateNotice(notice);

            await _noticeRepository.SaveAsync();

            var noticeDto = _mapper.Map<NoticeDto>(notice);

            return CreatedAtRoute(nameof(GetNotice), new { noticeId = notice.NoticeId }, noticeDto);
        }

        [HttpPatch(template: "{noticeId}")]
        public async Task<IActionResult> PartiallyUpdateNotice(Guid noticeId, JsonPatchDocument<NoticeUpdateDto> patchDocument)
        {
            if (!await _noticeRepository.NoticeExistAsync(noticeId))
            {
                //更新资源的Id不存在时，直接返回或是重新创建？ 
                return NotFound();
            }

            var noticeEntity = await _noticeRepository.GetNoticeAsync(noticeId);

            var noticePatchDto = _mapper.Map<NoticeUpdateDto>(noticeEntity);

            //需要处理验证错误
            patchDocument.ApplyTo(noticePatchDto, ModelState);

            if (!TryValidateModel(noticePatchDto))
            {
                return ValidationProblem(ModelState);
            }
            //剩下的步骤就是put步骤
            _mapper.Map(noticePatchDto, noticeEntity);

            _noticeRepository.UpdateNotice(noticeEntity);

            await _noticeRepository.SaveAsync();

            return NotFound();
        }
    }
}

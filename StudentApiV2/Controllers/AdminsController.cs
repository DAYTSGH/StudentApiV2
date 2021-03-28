using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
    public class AdminsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IAdminRepository _adminRepository;
        public AdminsController(IMapper mapper, IAdminRepository adminRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAdmins()
        {
            var admins = await _adminRepository.GetAdminsAsync();
            var adminDtos = _mapper.Map<IEnumerable<AdminDto>>(admins);
            return Ok(adminDtos);
        }

        [HttpGet]
        [Route(template: "{adminId}", Name = nameof(GetAdmin))]
        public async Task<ActionResult<AdminDto>> GetAdmin(Guid adminId)
        {
            var admin = await _adminRepository.GetAdminAsync(adminId);
            if (admin == null)
            {
                throw new ArgumentNullException(nameof(admin));
            }
            var adminDto = _mapper.Map<AdminDto>(admin);
            return Ok(adminDto);
        }

        [HttpPost]
        public async Task<ActionResult<AdminDto>> CreateAdmin([FromBody] AdminAddDto adminAddDto)
        {
            //ApiController在遇到adminAddDto为空时可以自动返回400错误
            var admin = _mapper.Map<Admin>(adminAddDto);
            _adminRepository.AddAdmin(admin);//只是被添加到DbContext里
            await _adminRepository.SaveAsync();

            var adminDto = _mapper.Map<AdminDto>(admin);
            return CreatedAtRoute(nameof(GetAdmin), new { adminId = admin.AdminId }, adminDto);
        }

        [HttpDelete("{adminId}", Name = nameof(DeleteAdmin))]
        public async Task<IActionResult> DeleteAdmin(Guid adminId)
        {
            var adminEntity = await _adminRepository.GetAdminAsync(adminId);

            if (adminEntity == null)
            {
                return NotFound();
            }

            _adminRepository.DeleteAdmin(adminEntity);
            await _adminRepository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{adminId}")]
        public async Task<ActionResult<AdminDto>> UpdateAdmin(Guid adminId, AdminUpdateDto adminUpdateDto)
        {
            if (!await _adminRepository.AdminExistAsync(adminId))
            {
                return NotFound();
            }

            var admin = await _adminRepository.GetAdminAsync(adminId);

            if (admin == null)
            {
                var adminToAdd = _mapper.Map<Admin>(adminUpdateDto);
                adminToAdd.AdminId = adminId;

                _adminRepository.AddAdmin(adminToAdd);

                await _adminRepository.SaveAsync();

                var adminDtoNew = _mapper.Map<AdminDto>(adminToAdd);
                return CreatedAtRoute(nameof(GetAdmin), new { adminId = admin.AdminId }, adminDtoNew);
            }

            _mapper.Map(adminUpdateDto, admin);

            _adminRepository.UpdateAdmin(admin);

            await _adminRepository.SaveAsync();

            var adminDto = _mapper.Map<AdminDto>(admin);

            return CreatedAtRoute(nameof(GetAdmin), new { adminId = admin.AdminId }, adminDto);
        }

        [HttpPut("password/{adminId}")]
        public async Task<ActionResult> UpdateAdminPassword(Guid adminId, PasswordUpdateDto passwordUpdateDto)
        {
            if (!await _adminRepository.AdminExistAsync(adminId))
            {
                return NotFound();
            }

            var admin = await _adminRepository.GetAdminAsync(adminId);
            if (admin != null)
            {
                if (MD5Helper.MD5Encode(passwordUpdateDto.OldPassword) == admin.AdminPassword)
                {
                    //输入密码正确，开始修改原密码
                    admin.AdminPassword = MD5Helper.MD5Encode(passwordUpdateDto.NewPassword);

                    _adminRepository.UpdatePassword(admin);
                    await _adminRepository.SaveAsync();
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpPatch(template: "{adminId}")]
        public async Task<IActionResult> PartiallyUpdateAdmin(Guid adminId, JsonPatchDocument<AdminUpdateDto> patchDocument)
        {
            if (!await _adminRepository.AdminExistAsync(adminId))
            {
                //更新资源的Id不存在时，直接返回或是重新创建？ 
                return NotFound();
            }

            var adminEntity = await _adminRepository.GetAdminAsync(adminId);

            var adminPatchDto = _mapper.Map<AdminUpdateDto>(adminEntity);

            //需要处理验证错误
            patchDocument.ApplyTo(adminPatchDto, ModelState);

            if (!TryValidateModel(adminPatchDto))
            {
                return ValidationProblem(ModelState);
            }
            //剩下的步骤就是put步骤
            _mapper.Map(adminPatchDto, adminEntity);

            _adminRepository.UpdateAdmin(adminEntity);

            await _adminRepository.SaveAsync();

            return NotFound();
        }
    }
}

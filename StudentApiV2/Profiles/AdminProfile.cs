using AutoMapper;
using StudentApiV2.AddDtos;
using StudentApiV2.Dtos;
using StudentApiV2.Entities;
using StudentApiV2.UpdateDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Profiles
{
    public class AdminProfile:Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, AdminDto>()
                .ForMember(dest => dest.AdminType, opt => opt.MapFrom(src => src.AdminType.ToString()));

            CreateMap<AdminAddDto, Admin>();

            CreateMap<AdminUpdateDto, Admin>();

            CreateMap<Admin, AdminUpdateDto>();
        }
    }
}

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
    public class ProfessionProfile:Profile
    {
        public ProfessionProfile()
        {
            CreateMap<Profession, ProfessionDto>().ForMember(x => x.AcademyName, opt => opt.MapFrom(src => src.Academy.AcademyName));

            CreateMap<ProfessionAddDto, Profession>();

            CreateMap<ProfessionUpdateDto, Profession>();

            CreateMap<Profession, ProfessionUpdateDto>();
        }
    }
}

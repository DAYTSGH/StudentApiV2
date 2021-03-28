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
    public class AcademyProfile:Profile
    {
        public AcademyProfile()
        {
            CreateMap<Academy, AcademyDto>();

            CreateMap<AcademyAddDto, Academy>();

            CreateMap<AcademyUpdateDto, Academy>();

            CreateMap<Academy,AcademyUpdateDto>();
        }
    }
}

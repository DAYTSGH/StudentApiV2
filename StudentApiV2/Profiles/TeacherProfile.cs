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
    public class TeacherProfile:Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherDto>().
                ForMember(dest => dest.TeacherGender, opt => opt.MapFrom(src => src.TeacherGender.ToString())).
                ForMember(dest => dest.TeacherTitle, opt => opt.MapFrom(src => src.TeacherTitle.ToString())).
                ForMember(dest => dest.AcademyName, opt => opt.MapFrom(src => src.Academy.AcademyName)).
                ForMember(dest => dest.TeacherType, opt => opt.MapFrom(src => src.TeacherType.ToString()));

            CreateMap<TeacherAddDto, Teacher>();
            CreateMap<TeacherUpdateDto, Teacher>();

            CreateMap<Teacher, TeacherUpdateDto>();
        }
    }
}

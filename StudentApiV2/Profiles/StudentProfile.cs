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
    public class StudentProfile:Profile
    {
        public StudentProfile() 
        {

            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.StudentGender, opt => opt.MapFrom(src => src.StudentGender.ToString()))
                .ForMember(dest => dest.ProfessionName, opt => opt.MapFrom
                   (src => src.Profession.ProfessionName))
                .ForMember(dest => dest.AcademyName, opt => opt.MapFrom
                   (src => src.Profession.Academy.AcademyName))
                .ForMember(dest => dest.StudentType, opt => opt.MapFrom(src => src.StudentType.ToString()));

            CreateMap<StudentAddDto, Student>();

            CreateMap<StudentUpdateDto, Student>();

            CreateMap<Student, StudentUpdateDto>();
        }
    }
}

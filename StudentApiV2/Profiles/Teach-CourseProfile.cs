using AutoMapper;
using StudentApiV2.AddDtos;
using StudentApiV2.Dtos;
using StudentApiV2.Entities;
using StudentApiV2.Services;
using StudentApiV2.UpdateDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Profiles
{
    public class Teach_CourseProfile:Profile
    {

        public Teach_CourseProfile()
        {
            CreateMap<Teach_Course, Teach_CourseDto>()
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course.CourseCode))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.CourseName))
                .ForMember(dest => dest.CourseHours, opt => opt.MapFrom(src => src.Course.CourseHours))
                .ForMember(dest => dest.CourseCredit, opt => opt.MapFrom(src => src.Course.CourseCredit))
                .ForMember(dest => dest.CourseType, opt => opt.MapFrom(src => src.Course.CourseType.ToString()))
                .ForMember(dest => dest.CourseType, opt => opt.MapFrom(src => src.Course.CourseType.ToString()))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.TeacherName))
                //.ForMember(dest => dest.TeacherGender, opt => opt.MapFrom(src => src.Teacher.TeacherGender.ToString()))
                .ForMember(dest => dest.AcademyName, opt => opt.MapFrom(src => src.Teacher.Academy.AcademyName));

            CreateMap<Teach_CourseAddDto, Teach_Course>();

            CreateMap<Teach_CourseUpdateDto, Teach_Course>();

            CreateMap<Teach_Course, Teach_CourseUpdateDto>();

        }
    }
}

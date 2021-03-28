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
    public class ScoreProfile:Profile
    {
        public ScoreProfile()
        {
            CreateMap<Score, ScoreDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.StudentName))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Teach_Course.Course.CourseName))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teach_Course.Teacher.TeacherName))
                .ForMember(dest => dest.CourseType, opt => opt.MapFrom(src => src.Teach_Course.Course.CourseType.ToString()))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Teach_Course.Course.CourseCode))
                .ForMember(dest => dest.CourseCredit, opt => opt.MapFrom(src => src.Teach_Course.Course.CourseCredit))
                .ForMember(dest => dest.CourseHours, opt => opt.MapFrom(src => src.Teach_Course.Course.CourseHours))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student.StudentCode))
                .ForMember(dest => dest.IsMarked, opt => opt.MapFrom(src => src.Teach_Course.IsMarked));

            CreateMap<ScoreAddDto, Score>();

            CreateMap<ScoreUpdateDto, Score>();

            CreateMap<Score, ScoreUpdateDto>();
        }
    }
}

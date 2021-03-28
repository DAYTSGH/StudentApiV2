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
    public class CourseProfile:Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>().ForMember(dest => dest.CourseType, opt => opt.MapFrom(src => src.CourseType.ToString()))
                .ForMember(dest => dest.ImageSource, opt => opt.MapFrom(src => "http://qkwyrqal4.hn-bkt.clouddn.com/"+src.ImageSource));

            CreateMap<CourseAddDto, Course>();

            CreateMap<CourseUpdateDto, Course>();

            CreateMap<Course, CourseUpdateDto>();
        }
    }
}

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
    public class NoticeProfile:Profile
    {
        public NoticeProfile()
        {
            CreateMap<Notice, NoticeDto>()
                .ForMember(dest => dest.EditTime, opt => opt.MapFrom(src => src.EditTime.ToString("yyyy/MM/dd")))
                .ForMember(dest => dest.PhotoSource, opt => opt.MapFrom(src => "http://qkwyrqal4.hn-bkt.clouddn.com/" + src.PhotoSource));

            CreateMap<NoticeAddDto, Notice>();

            CreateMap<NoticeUpdateDto, Notice>();

            CreateMap<Notice, NoticeUpdateDto>();
        }
    }
}

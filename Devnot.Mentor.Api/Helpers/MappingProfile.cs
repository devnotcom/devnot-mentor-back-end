using AutoMapper;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<MentorProfileModel, Mentor>()
                .ForMember(dest => dest.MentorTags, opt => opt.Ignore())
                .ForMember(dest => dest.MentorLinks, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<MenteeProfileModel, Mentee>()
                .ForMember(dest => dest.MenteeTags, opt => opt.Ignore())
                .ForMember(dest => dest.MenteeLinks, opt => opt.Ignore())
                .ReverseMap();
        }
    }

    // CreateMap<string, MentorLinks>().ConvertUsing<MentorLinksConvertor>();
    //public class MentorLinksConvertor : ITypeConverter<string, MentorLinks>
    //{
    //    public MentorLinks Convert(string source, MentorLinks destination, ResolutionContext context)
    //    {
    //        return new MentorLinks { Link = source };
    //    }
    //}

}

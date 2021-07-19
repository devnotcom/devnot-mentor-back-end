using AutoMapper;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.OAuth;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;
using DevnotMentor.Api.Entities;

namespace DevnotMentor.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Mentee, MenteeDto>();
            CreateMap<Mentor, MentorDto>();

            CreateMap<User, UserDto>();
            CreateMap<OAuthUser, User>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<MentorApplications, MentorApplicationsDto>();

            CreateMap<CreateMentorProfileRequest, Mentor>()
                .ForMember(dest => dest.MentorTags, opt => opt.Ignore())
                .ForMember(dest => dest.MentorLinks, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CreateMenteeProfileRequest, Mentee>()
                .ForMember(dest => dest.MenteeTags, opt => opt.Ignore())
                .ForMember(dest => dest.MenteeLinks, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}

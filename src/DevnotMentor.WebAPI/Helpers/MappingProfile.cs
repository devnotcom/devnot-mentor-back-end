using AutoMapper;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentee;
using DevnotMentor.Common.Requests.Mentor;
using DevnotMentor.Data.Entities;
using DevnotMentor.Common.Requests.User;

namespace DevnotMentor.WebAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Mentee, MenteeDTO>();
            CreateMap<Mentor, MentorDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<Application, ApplicationDTO>();
            CreateMap<Mentorship, MentorshipDTO>();

            CreateMap<CreateMentorProfileRequest, Mentor>()
                .ForMember(dest => dest.MentorTags, opt => opt.Ignore())
                .ForMember(dest => dest.MentorLinks, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CreateMenteeProfileRequest, Mentee>()
                .ForMember(dest => dest.MenteeTags, opt => opt.Ignore())
                .ForMember(dest => dest.MenteeLinks, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<RegisterUserRequest, User>();

            CreateMap<Tag, TagDTO>();
            CreateMap<MentorTag, MentorTagDTO>();
            CreateMap<MenteeTag, MenteeTagDTO>();
        }
    }
}

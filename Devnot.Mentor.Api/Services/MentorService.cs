using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;
using System.Collections.Generic;
using DevnotMentor.Api.CustomEntities.Request.CommonRequest;

namespace DevnotMentor.Api.Services
{
    public class MentorService : BaseService, IMentorService
    {
        private readonly IMentorRepository mentorRepository;
        private readonly IMenteeRepository menteeRepository;
        private readonly IMentorLinksRepository mentorLinksRepository;
        private readonly IMentorTagsRepository mentorTagsRepository;
        private readonly ITagRepository tagRepository;
        private readonly IUserRepository userRepository;
        private readonly IMentorApplicationsRepository applicationsRepository;
        private readonly IMentorMenteePairsRepository pairsRepository;

        public MentorService(
            IMapper mapper,
            IMentorRepository mentorRepository,
            IMenteeRepository menteeRepository,
            IMentorLinksRepository mentorLinksRepository,
            IMentorTagsRepository mentorTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IMentorApplicationsRepository mentorApplicationsRepository,
            IMentorMenteePairsRepository mentorMenteePairsRepository,
            ILoggerRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext
            ) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            this.mentorRepository = mentorRepository;
            this.menteeRepository = menteeRepository;
            this.mentorLinksRepository = mentorLinksRepository;
            this.mentorTagsRepository = mentorTagsRepository;
            this.tagRepository = tagRepository;
            this.userRepository = userRepository;
            this.applicationsRepository = mentorApplicationsRepository;
            this.pairsRepository = mentorMenteePairsRepository;
        }

        public async Task<ApiResponse<MentorDto>> GetMentorProfileAsync(string userName)
        {
            var mentor = await mentorRepository.GetByUserNameAsync(userName);

            if (mentor == null)
            {
                return new ErrorApiResponse<MentorDto>(ResponseStatus.NotFound, data: default, ResultMessage.NotFoundMentor);
            }

            var mappedMentor = mapper.Map<MentorDto>(mentor);
            return new SuccessApiResponse<MentorDto>(mappedMentor);
        }

        public async Task<ApiResponse<List<MenteeDto>>> GetPairedMenteesByUserIdAsync(int userId)
        {
            var mentor = await mentorRepository.GetByUserIdAsync(userId);

            if (mentor == null)
            {
                return new ErrorApiResponse<List<MenteeDto>>(ResponseStatus.NotFound, data: default, ResultMessage.NotFoundMentor);
            }

            var pairedMentees = mapper.Map<List<MenteeDto>>(await mentorRepository.GetPairedMenteesByMentorIdAsync(mentor.Id));

            return new SuccessApiResponse<List<MenteeDto>>(pairedMentees);
        }

        public async Task<ApiResponse<MentorDto>> CreateMentorProfileAsync(CreateMentorProfileRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                return new ErrorApiResponse<MentorDto>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundUser);
            }

            var registeredMentor = await mentorRepository.GetByUserIdAsync(user.Id);

            if (registeredMentor != null)
            {
                return new ErrorApiResponse<MentorDto>(data: default, message: ResultMessage.MentorAlreadyRegistered);
            }

            var mentor = CreateNewMentor(request, user);

            if (mentor == null)
            {
                return new ErrorApiResponse<MentorDto>(data: default, message: ResultMessage.FailedToAddMentor);
            }

            var mappedMentor = mapper.Map<MentorDto>(mentor);
            return new SuccessApiResponse<MentorDto>(mappedMentor);
        }

        private Mentor CreateNewMentor(CreateMentorProfileRequest request, User user)
        {
            Mentor mentor = null;

            var newMentor = mapper.Map<CreateMentorProfileRequest, Mentor>(request);

            newMentor.UserId = user.Id;

            mentor = mentorRepository.Create(newMentor);

            if (mentor == null)
            {
                return null;
            }

            mentorLinksRepository.Create(mentor.Id, request.MentorLinks);

            foreach (var mentorTag in request.MentorTags)
            {
                if (String.IsNullOrWhiteSpace(mentorTag))
                {
                    continue;
                }

                var tag = tagRepository.Get(mentorTag);

                if (tag != null)
                {
                    mentorTagsRepository.Create(new MentorTags { TagId = tag.Id, MentorId = mentor.Id });
                }
                else
                {
                    var newTag = tagRepository.Create(new Tag { Name = mentorTag });

                    if (newTag != null)
                    {
                        mentorTagsRepository.Create(new MentorTags { TagId = newTag.Id, MentorId = mentor.Id });
                    }
                }
            }

            return mentor;
        }
        
        public async Task<ApiResponse<List<MentorDto>>> SearchAsync(SearchRequest request)
        {
            var mappedMentors = mapper.Map<List<MentorDto>>(await mentorRepository.SearchAsync(request));
            return new SuccessApiResponse<List<MentorDto>>(mappedMentors);
        }
    }
}

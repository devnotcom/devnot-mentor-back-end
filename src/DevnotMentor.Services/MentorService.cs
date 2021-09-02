using AutoMapper;
using DevnotMentor.Common;
using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Services.Interfaces;
using System;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentor;
using System.Collections.Generic;
using DevnotMentor.Common.Requests;

namespace DevnotMentor.Services
{
    public class MentorService : BaseService, IMentorService
    {
        private readonly IMentorRepository mentorRepository;
        private readonly IMenteeRepository menteeRepository;
        private readonly IMentorLinkRepository mentorLinksRepository;
        private readonly IMentorTagRepository mentorTagsRepository;
        private readonly ITagRepository tagRepository;
        private readonly IUserRepository userRepository;
        private readonly IApplicationsRepository applicationsRepository;
        private readonly IMentorshipsRepository pairsRepository;

        public MentorService(
            IMapper mapper,
            IMentorRepository mentorRepository,
            IMenteeRepository menteeRepository,
            IMentorLinkRepository mentorLinksRepository,
            IMentorTagRepository mentorTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IApplicationsRepository mentorApplicationsRepository,
            IMentorshipsRepository MentorshipsRepository,
            ILogRepository loggerRepository,
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
            this.pairsRepository = MentorshipsRepository;
        }

        public async Task<ApiResponse<MentorDTO>> GetMentorProfileAsync(string userName)
        {
            var mentor = await mentorRepository.GetByUserNameAsync(userName);

            if (mentor == null)
            {
                return new ErrorApiResponse<MentorDTO>(ResponseStatus.NotFound, data: default, ResultMessage.NotFoundMentor);
            }

            var mappedMentor = mapper.Map<MentorDTO>(mentor);
            return new SuccessApiResponse<MentorDTO>(mappedMentor);
        }

        public async Task<ApiResponse<List<MenteeDTO>>> GetPairedMenteesByUserIdAsync(int userId)
        {
            var mentor = await mentorRepository.GetByUserIdAsync(userId);

            if (mentor == null)
            {
                return new ErrorApiResponse<List<MenteeDTO>>(ResponseStatus.NotFound, data: default, ResultMessage.NotFoundMentor);
            }

            var pairedMentees = mapper.Map<List<MenteeDTO>>(await mentorRepository.GetPairedMenteesByMentorIdAsync(mentor.Id));

            return new SuccessApiResponse<List<MenteeDTO>>(pairedMentees);
        }

        public async Task<ApiResponse<MentorDTO>> CreateMentorProfileAsync(CreateMentorProfileRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                return new ErrorApiResponse<MentorDTO>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundUser);
            }

            var registeredMentor = await mentorRepository.GetByUserIdAsync(user.Id);

            if (registeredMentor != null)
            {
                return new ErrorApiResponse<MentorDTO>(data: default, message: ResultMessage.MentorAlreadyRegistered);
            }

            var mentor = CreateNewMentor(request, user);

            if (mentor == null)
            {
                return new ErrorApiResponse<MentorDTO>(data: default, message: ResultMessage.FailedToAddMentor);
            }

            var mappedMentor = mapper.Map<MentorDTO>(mentor);
            return new SuccessApiResponse<MentorDTO>(mappedMentor);
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
                    mentorTagsRepository.Create(new MentorTag { TagId = tag.Id, MentorId = mentor.Id });
                }
                else
                {
                    var newTag = tagRepository.Create(new Tag { Name = mentorTag });

                    if (newTag != null)
                    {
                        mentorTagsRepository.Create(new MentorTag { TagId = newTag.Id, MentorId = mentor.Id });
                    }
                }
            }

            return mentor;
        }

        public async Task<ApiResponse<List<MentorDTO>>> SearchAsync(SearchRequest request)
        {
            var mappedMentors = mapper.Map<List<MentorDTO>>(await mentorRepository.SearchAsync(request));
            return new SuccessApiResponse<List<MentorDTO>>(mappedMentors);
        }

    }
}

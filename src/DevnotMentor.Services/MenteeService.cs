using AutoMapper;
using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Services.Interfaces;
using System;
using System.Threading.Tasks;
using DevnotMentor.Common;
using DevnotMentor.Common.API;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentee;
using System.Collections.Generic;
using DevnotMentor.Common.Requests;
namespace DevnotMentor.Services
{
    public class MenteeService : BaseService, IMenteeService
    {
        private readonly IMenteeRepository menteeRepository;
        private readonly IMenteeLinksRepository menteeLinksRepository;
        private readonly IMenteeTagsRepository menteeTagsRepository;
        private readonly ITagRepository tagRepository;
        private readonly IUserRepository userRepository;
        private readonly IMentorRepository mentorRepository;
        private readonly IApplicationsRepository applicationsRepository;
        private readonly IMentorshipsRepository pairsRepository;

        public MenteeService(
            IMapper mapper,
            IMenteeRepository menteeRepository,
            IMenteeLinksRepository menteeLinksRepository,
            IMenteeTagsRepository menteeTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IMentorRepository mentorRepository,
            IApplicationsRepository mentorApplicationsRepository,
            IMentorshipsRepository MentorshipsRepository,
            ILogRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext
            )
            : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            this.menteeRepository = menteeRepository;
            this.menteeLinksRepository = menteeLinksRepository;
            this.menteeTagsRepository = menteeTagsRepository;
            this.tagRepository = tagRepository;
            this.userRepository = userRepository;
            this.mentorRepository = mentorRepository;
            this.applicationsRepository = mentorApplicationsRepository;
            this.pairsRepository = MentorshipsRepository;
            
        }

        public async Task<ApiResponse<MenteeDTO>> GetMenteeProfileAsync(string userName)
        {
            var mentee = await menteeRepository.GetByUserNameAsync(userName);

            if (mentee == null)
            {
                return new ErrorApiResponse<MenteeDTO>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentee);
            }

            var mappedMentee = mapper.Map<Mentee, MenteeDTO>(mentee);
            return new SuccessApiResponse<MenteeDTO>(mappedMentee);
        }

        public async Task<ApiResponse<List<MentorDTO>>> GetPairedMentorsByUserIdAsync(int userId)
        {
            var mentee = await menteeRepository.GetByUserIdAsync(userId);

            if (mentee == null)
            {
                return new ErrorApiResponse<List<MentorDTO>>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentee);
            }

            var pairedMentors = mapper.Map<List<MentorDTO>>(await menteeRepository.GetPairedMentorsByMenteeIdAsync(mentee.Id));
            return new SuccessApiResponse<List<MentorDTO>>(pairedMentors);
        }

        public async Task<ApiResponse<MenteeDTO>> CreateMenteeProfileAsync(CreateMenteeProfileRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                return new ErrorApiResponse<MenteeDTO>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundUser);
            }

            var registeredMentee = await menteeRepository.GetByUserIdAsync(user.Id);

            if (registeredMentee != null)
            {
                return new ErrorApiResponse<MenteeDTO>(data: default, message: ResultMessage.MenteeAlreadyRegistered);
            }

            var mentee = CreateNewMentee(request, user);

            if (mentee == null)
            {
                return new ErrorApiResponse<MenteeDTO>(data: default, ResultMessage.FailedToAddMentee);
            }

            var mappedMentee = mapper.Map<MenteeDTO>(mentee);
            return new SuccessApiResponse<MenteeDTO>(mappedMentee);
        }

        private Mentee CreateNewMentee(CreateMenteeProfileRequest request, User user)
        {
            Mentee mentee = null;

            var newMentee = mapper.Map<Mentee>(request);
            newMentee.UserId = user.Id;

            mentee = menteeRepository.Create(newMentee);

            if (mentee == null)
            {
                return null;
            }

            menteeLinksRepository.Create(mentee.Id, request.MenteeLinks);

            foreach (var menteeTag in request.MenteeTags)
            {
                if (String.IsNullOrWhiteSpace(menteeTag))
                {
                    continue;
                }

                var tag = tagRepository.Get(menteeTag);

                if (tag != null)
                {
                    menteeTagsRepository.Create(new MenteeTag { TagId = tag.Id, MenteeId = mentee.Id });
                }
                else
                {
                    var newTag = tagRepository.Create(new Tag { Name = menteeTag });

                    if (newTag != null)
                    {
                        menteeTagsRepository.Create(new MenteeTag { TagId = newTag.Id, MenteeId = mentee.Id });
                    }
                }
            }

            return mentee;
        }
        public async Task<ApiResponse<List<MenteeDTO>>> SearchAsync(SearchRequest request)
        {
            var mappedMentees = mapper.Map<List<MenteeDTO>>(await menteeRepository.SearchAsync(request));
            return new SuccessApiResponse<List<MenteeDTO>>(mappedMentees);
        }

    }
}

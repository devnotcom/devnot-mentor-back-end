using AutoMapper;
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
        private readonly IMentorRepository _mentorRepository;
        private readonly IMenteeRepository _menteeRepository;
        private readonly IMentorLinkRepository _mentorLinkRepository;
        private readonly IMentorTagRepository _mentorTagRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMentorshipRepository _mentorshipRepository;

        public MentorService(
            IMapper mapper,
            IMentorRepository mentorRepository,
            IMenteeRepository menteeRepository,
            IMentorLinkRepository mentorLinksRepository,
            IMentorTagRepository mentorTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IApplicationRepository mentorApplicationsRepository,
            IMentorshipRepository MentorshipsRepository,
            ILogRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext
            ) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            _mentorRepository = mentorRepository;
            _menteeRepository = menteeRepository;
            _mentorLinkRepository = mentorLinksRepository;
            _mentorTagRepository = mentorTagsRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _applicationRepository = mentorApplicationsRepository;
            _mentorshipRepository = MentorshipsRepository;
        }

        public async Task<ApiResponse<MentorDTO>> GetMentorProfileAsync(string userName)
        {
            var mentor = await _mentorRepository.GetByUserNameAsync(userName);

            if (mentor == null)
            {
                return new ErrorApiResponse<MentorDTO>(ResponseStatus.NotFound, data: default, ResultMessage.NotFoundMentor);
            }

            var mappedMentor = mapper.Map<MentorDTO>(mentor);
            return new SuccessApiResponse<MentorDTO>(mappedMentor);
        }

        public async Task<ApiResponse<List<MenteeDTO>>> GetPairedMenteesByUserIdAsync(int userId)
        {
            var mentor = await _mentorRepository.GetByUserIdAsync(userId);

            if (mentor == null)
            {
                return new ErrorApiResponse<List<MenteeDTO>>(ResponseStatus.NotFound, data: default, ResultMessage.NotFoundMentor);
            }

            var pairedMentees = mapper.Map<List<MenteeDTO>>(await _mentorRepository.GetPairedMenteesByMentorIdAsync(mentor.Id));

            return new SuccessApiResponse<List<MenteeDTO>>(pairedMentees);
        }

        public async Task<ApiResponse<MentorDTO>> CreateMentorProfileAsync(CreateMentorProfileRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                return new ErrorApiResponse<MentorDTO>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundUser);
            }

            var registeredMentor = await _mentorRepository.GetByUserIdAsync(user.Id);

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

            mentor = _mentorRepository.Create(newMentor);

            if (mentor == null)
            {
                return null;
            }

            _mentorLinkRepository.Create(mentor.Id, request.MentorLinks);

            foreach (var mentorTag in request.MentorTags)
            {
                if (String.IsNullOrWhiteSpace(mentorTag))
                {
                    continue;
                }

                var tag = _tagRepository.Get(mentorTag);

                if (tag != null)
                {
                    _mentorTagRepository.Create(new MentorTag { TagId = tag.Id, MentorId = mentor.Id });
                }
                else
                {
                    var newTag = _tagRepository.Create(new Tag { Name = mentorTag });

                    if (newTag != null)
                    {
                        _mentorTagRepository.Create(new MentorTag { TagId = newTag.Id, MentorId = mentor.Id });
                    }
                }
            }

            return mentor;
        }

        public async Task<ApiResponse<List<MentorDTO>>> SearchAsync(SearchRequest request)
        {
            var mappedMentors = mapper.Map<List<MentorDTO>>(await _mentorRepository.SearchAsync(request));
            return new SuccessApiResponse<List<MentorDTO>>(mappedMentors);
        }
    }
}

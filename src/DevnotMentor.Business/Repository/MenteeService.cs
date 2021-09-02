using AutoMapper;
using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Business.Repository.Interfaces;
using System;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentee;
using System.Collections.Generic;
using DevnotMentor.Common.Requests;
namespace DevnotMentor.Business.Repository
{
    public class MenteeService : BaseService, IMenteeService
    {
        private readonly IMenteeRepository _menteeRepository;
        private readonly IMenteeLinkRepository _menteeLinkRepository;
        private readonly IMenteeTagRepository _menteeTagsRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMentorRepository _mentorRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMentorshipRepository _mentorshipRepository;

        public MenteeService(
            IMapper mapper,
            IMenteeRepository menteeRepository,
            IMenteeLinkRepository menteeLinksRepository,
            IMenteeTagRepository menteeTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IMentorRepository mentorRepository,
            IApplicationRepository mentorApplicationsRepository,
            IMentorshipRepository MentorshipsRepository,
            ILogRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext
            )
            : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            _menteeRepository = menteeRepository;
            _menteeLinkRepository = menteeLinksRepository;
            _menteeTagsRepository = menteeTagsRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _mentorRepository = mentorRepository;
            _applicationRepository = mentorApplicationsRepository;
            _mentorshipRepository = MentorshipsRepository;
        }

        public async Task<ApiResponse<MenteeDTO>> GetMenteeProfileByUserNameAsync(string userName)
        {
            var mentee = await _menteeRepository.GetByUserNameAsync(userName);
            if (mentee == null)
            {
                return new ErrorApiResponse<MenteeDTO>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentee);
            }

            var mappedMentee = mapper.Map<Mentee, MenteeDTO>(mentee);
            return new SuccessApiResponse<MenteeDTO>(mappedMentee);
        }

        public async Task<ApiResponse<List<MentorDTO>>> GetPairedMentorsByUserIdAsync(int userId)
        {
            var mentee = await _menteeRepository.GetByUserIdAsync(userId);
            if (mentee == null)
            {
                return new ErrorApiResponse<List<MentorDTO>>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentee);
            }

            var mentorsPairedToMentee = mapper.Map<List<MentorDTO>>(await _menteeRepository.GetPairedMentorsByMenteeIdAsync(mentee.Id));
            return new SuccessApiResponse<List<MentorDTO>>(mentorsPairedToMentee);
        }

        public async Task<ApiResponse<MenteeDTO>> CreateMenteeProfileAsync(CreateMenteeProfileRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new ErrorApiResponse<MenteeDTO>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundUser);
            }

            var userAlreadyMentee = await _menteeRepository.GetByUserIdAsync(user.Id) != null;
            if (userAlreadyMentee)
            {
                return new ErrorApiResponse<MenteeDTO>(data: default, message: ResultMessage.MenteeAlreadyRegistered);
            }

            var newMentee = CreateNewMentee(request, user);
            if (newMentee == null)
            {
                return new ErrorApiResponse<MenteeDTO>(data: default, ResultMessage.FailedToAddMentee);
            }

            var mappedNewMentee = mapper.Map<MenteeDTO>(newMentee);
            return new SuccessApiResponse<MenteeDTO>(ResponseStatus.Created, mappedNewMentee);
        }

        private Mentee CreateNewMentee(CreateMenteeProfileRequest request, User user)
        {
            Mentee createdNewMentee = null;

            var newMentee = mapper.Map<Mentee>(request);
            newMentee.UserId = user.Id;

            createdNewMentee = _menteeRepository.Create(newMentee);

            if (createdNewMentee == null)
            {
                return null;
            }

            _menteeLinkRepository.Create(createdNewMentee.Id, request.MenteeLinks);

            foreach (var menteeTag in request.MenteeTags)
            {
                if (String.IsNullOrWhiteSpace(menteeTag))
                {
                    continue;
                }

                var tag = _tagRepository.GetByName(menteeTag);
                if (tag != null)
                {
                    _menteeTagsRepository.Create(new MenteeTag { TagId = tag.Id, MenteeId = createdNewMentee.Id });
                }
                else
                {
                    var newTag = _tagRepository.Create(new Tag { Name = menteeTag });
                    if (newTag != null)
                    {
                        _menteeTagsRepository.Create(new MenteeTag { TagId = newTag.Id, MenteeId = createdNewMentee.Id });
                    }
                }
            }

            return createdNewMentee;
        }

        public async Task<ApiResponse<List<MenteeDTO>>> SearchAsync(SearchRequest request)
        {
            var mappedMentees = mapper.Map<List<MenteeDTO>>(await _menteeRepository.SearchAsync(request));
            return new SuccessApiResponse<List<MenteeDTO>>(mappedMentees);
        }
    }
}

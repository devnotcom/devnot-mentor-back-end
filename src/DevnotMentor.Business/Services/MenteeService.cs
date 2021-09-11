using AutoMapper;
using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Business.Services.Interfaces;
using System;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Configuration.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentee;
using System.Collections.Generic;
using DevnotMentor.Common.Requests;

namespace DevnotMentor.Business.Services
{
    public class MenteeService : BaseService, IMenteeService
    {
        private readonly IMenteeRepository _menteeRepository;
        private readonly IMenteeLinkRepository _menteeLinkRepository;
        private readonly IMenteeTagRepository _menteeTagsRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;

        public MenteeService(
            IMapper mapper,
            IMenteeRepository menteeRepository,
            IMenteeLinkRepository menteeLinksRepository,
            IMenteeTagRepository menteeTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
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
        }

        public async Task<ApiResponse<MenteeDTO>> GetMenteeProfileByUserNameAsync(string userName)
        {
            var mentee = await _menteeRepository.GetByUserNameAsync(userName);
            if (mentee == null)
            {
                return new ErrorApiResponse<MenteeDTO>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentee);
            }

            var mappedMentee = _mapper.Map<Mentee, MenteeDTO>(mentee);
            return new SuccessApiResponse<MenteeDTO>(mappedMentee);
        }

        public async Task<ApiResponse<List<MentorDTO>>> GetPairedMentorsByUserIdAsync(int userId)
        {
            var mentee = await _menteeRepository.GetByUserIdAsync(userId);
            if (mentee == null)
            {
                return new ErrorApiResponse<List<MentorDTO>>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentee);
            }

            var mentorsPairedToMentee = _mapper.Map<List<MentorDTO>>(await _menteeRepository.GetPairedMentorsByMenteeIdAsync(mentee.Id));
            return new SuccessApiResponse<List<MentorDTO>>(mentorsPairedToMentee);
        }

        public async Task<ApiResponse<MenteeDTO>> CreateMenteeProfileAsync(CreateMenteeProfileRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new ErrorApiResponse<MenteeDTO>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundUser);
            }

            var userAlreadyMentee = user.IsMentee || await _menteeRepository.GetByUserIdAsync(user.Id) != null;
            if (userAlreadyMentee)
            {
                return new ErrorApiResponse<MenteeDTO>(data: default, message: ResultMessage.MenteeAlreadyRegistered);
            }

            var newMentee = CreateNewMentee(request, user);
            if (newMentee == null)
            {
                return new ErrorApiResponse<MenteeDTO>(data: default, ResultMessage.FailedToAddMentee);
            }

            user.IsMentee = true;
            _userRepository.Update(user);

            var mappedNewMentee = _mapper.Map<MenteeDTO>(newMentee);
            return new SuccessApiResponse<MenteeDTO>(ResponseStatus.Created, mappedNewMentee);
        }

        private Mentee CreateNewMentee(CreateMenteeProfileRequest request, User user)
        {
            Mentee createdNewMentee = null;

            var newMentee = _mapper.Map<Mentee>(request);
            newMentee.UserId = user.Id;

            createdNewMentee = _menteeRepository.Create(newMentee);

            if (createdNewMentee == null)
            {
                return null;
            }

            if (request.MenteeLinks != null)
            {
                _menteeLinkRepository.Create(createdNewMentee.Id, request.MenteeLinks);
            }

            if (request.MenteeTags != null)
            {
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
            }

            return createdNewMentee;
        }

        public async Task<ApiResponse<List<MenteeDTO>>> SearchAsync(SearchRequest request)
        {
            var mappedMentees = _mapper.Map<List<MenteeDTO>>(await _menteeRepository.SearchAsync(request));
            return new SuccessApiResponse<List<MenteeDTO>>(mappedMentees);
        }
    }
}

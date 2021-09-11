using AutoMapper;
using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Business.Services.Interfaces;
using System;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Configuration.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentor;
using System.Collections.Generic;
using DevnotMentor.Common.Requests;

namespace DevnotMentor.Business.Services
{
    public class MentorService : BaseService, IMentorService
    {
        private readonly IMentorLinkRepository _mentorLinkRepository;
        private readonly IMentorTagRepository _mentorTagRepository;
        private readonly IMentorRepository _mentorRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;

        public MentorService(
            IMapper mapper,
            IMentorRepository mentorRepository,
            IMentorLinkRepository mentorLinksRepository,
            IMentorTagRepository mentorTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            ILogRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext
            ) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            _mentorRepository = mentorRepository;
            _mentorLinkRepository = mentorLinksRepository;
            _mentorTagRepository = mentorTagsRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<MentorDTO>> GetMentorProfileByUserNameAsync(string userName)
        {
            var mentor = await _mentorRepository.GetByUserNameAsync(userName);
            if (mentor == null)
            {
                return new ErrorApiResponse<MentorDTO>(ResponseStatus.NotFound, data: default, ResultMessage.NotFoundMentor);
            }

            var mappedMentor = _mapper.Map<MentorDTO>(mentor);
            return new SuccessApiResponse<MentorDTO>(mappedMentor);
        }

        public async Task<ApiResponse<List<MenteeDTO>>> GetPairedMenteesByUserIdAsync(int userId)
        {
            var mentor = await _mentorRepository.GetByUserIdAsync(userId);
            if (mentor == null)
            {
                return new ErrorApiResponse<List<MenteeDTO>>(ResponseStatus.NotFound, data: default, ResultMessage.NotFoundMentor);
            }

            var menteesPairedToMentor = _mapper.Map<List<MenteeDTO>>(await _mentorRepository.GetPairedMenteesByMentorIdAsync(mentor.Id));
            return new SuccessApiResponse<List<MenteeDTO>>(menteesPairedToMentor);
        }

        public async Task<ApiResponse<MentorDTO>> CreateMentorProfileAsync(CreateMentorProfileRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new ErrorApiResponse<MentorDTO>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundUser);
            }

            var userAlreadyMentor = user.IsMentor || await _mentorRepository.GetByUserIdAsync(user.Id) != null;
            if (userAlreadyMentor)
            {
                return new ErrorApiResponse<MentorDTO>(data: default, message: ResultMessage.MentorAlreadyRegistered);
            }

            var newMentor = CreateNewMentor(request, user);
            if (newMentor == null)
            {
                return new ErrorApiResponse<MentorDTO>(data: default, message: ResultMessage.FailedToAddMentor);
            }

            user.IsMentor = true;
            _userRepository.Update(user);

            var mappedNewMentor = _mapper.Map<MentorDTO>(newMentor);
            return new SuccessApiResponse<MentorDTO>(ResponseStatus.Created, mappedNewMentor);
        }

        private Mentor CreateNewMentor(CreateMentorProfileRequest request, User user)
        {
            Mentor createdNewMentor = null;

            var newMentor = _mapper.Map<CreateMentorProfileRequest, Mentor>(request);
            newMentor.UserId = user.Id;

            createdNewMentor = _mentorRepository.Create(newMentor);
            if (createdNewMentor == null)
            {
                return null;
            }

            if (request.MentorLinks != null)
            {
                _mentorLinkRepository.Create(createdNewMentor.Id, request.MentorLinks);
            }

            if (request.MentorTags != null)
            {
                foreach (var mentorTag in request.MentorTags)
                {
                    if (String.IsNullOrWhiteSpace(mentorTag))
                    {
                        continue;
                    }

                    var tag = _tagRepository.GetByName(mentorTag);
                    if (tag != null)
                    {
                        _mentorTagRepository.Create(new MentorTag { TagId = tag.Id, MentorId = createdNewMentor.Id });
                    }
                    else
                    {
                        var newTag = _tagRepository.Create(new Tag { Name = mentorTag });
                        if (newTag != null)
                        {
                            _mentorTagRepository.Create(new MentorTag { TagId = newTag.Id, MentorId = createdNewMentor.Id });
                        }
                    }
                }
            }

            return createdNewMentor;
        }

        public async Task<ApiResponse<List<MentorDTO>>> SearchAsync(SearchRequest request)
        {
            var mappedMentors = _mapper.Map<List<MentorDTO>>(await _mentorRepository.SearchAsync(request));
            return new SuccessApiResponse<List<MentorDTO>>(mappedMentors);
        }
    }
}

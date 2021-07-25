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
                return new ErrorApiResponse<MentorDto>(data: default, ResultMessage.NotFoundMentor);
            }

            var mappedMentor = mapper.Map<MentorDto>(mentor);
            return new SuccessApiResponse<MentorDto>(mappedMentor);
        }

        public async Task<ApiResponse<List<MenteeDto>>> GetPairedMenteesByUserIdAsync(int userId)
        {
            var mentor = await mentorRepository.GetByUserIdAsync(userId);

            if (mentor == null)
            {
                return new ErrorApiResponse<List<MenteeDto>>(data: default, ResultMessage.NotFoundMentor);
            }

            var pairedMentees = mapper.Map<List<MenteeDto>>(await mentorRepository.GetPairedMenteesByMentorIdAsync(mentor.Id));

            return new SuccessApiResponse<List<MenteeDto>>(pairedMentees);
        }
        public async Task<ApiResponse<List<PairsDto>>> GetMentorshipsByUserIdAsync(int userId)
        {
            var mentor = await mentorRepository.GetByUserIdAsync(userId);

            if (mentor == null)
            {
                return new ErrorApiResponse<List<PairsDto>>(data: default, message: ResultMessage.NotFoundMentor);
            }

            var pairs = mapper.Map<List<PairsDto>>(await pairsRepository.GetListByUserIdAsync(userId));

            return new SuccessApiResponse<List<PairsDto>>(pairs);
        }

        public async Task<ApiResponse<List<MentorApplicationsDto>>> GetApplicationsByUserIdAsync(int userId)
        {
            var mentor = await mentorRepository.GetByUserIdAsync(userId);

            if (mentor == null)
            {
                return new ErrorApiResponse<List<MentorApplicationsDto>>(data: default, message: ResultMessage.NotFoundMentor);
            }

            var applications = mapper.Map<List<MentorApplicationsDto>>(await applicationsRepository.GetByUserIdAsync(userId));

            return new SuccessApiResponse<List<MentorApplicationsDto>>(applications);
        }

        public async Task<ApiResponse<MentorDto>> CreateMentorProfileAsync(CreateMentorProfileRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                return new ErrorApiResponse<MentorDto>(data: default, message: ResultMessage.NotFoundUser);
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

        public async Task<ApiResponse> AcceptMenteeAsync(int mentorUserId, int mentorId, int menteeId)
        {
            var mentor = await mentorRepository.GetByUserIdAsync(mentorUserId);

            if (mentor == null || mentor.Id != mentorId)
            {
                return new ErrorApiResponse(ResultMessage.UnAuthorized);
            }

            var mentorApplication = await applicationsRepository.GetAsync(mentorId, menteeId);

            if (mentorApplication == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentorMenteePair);
            }

            if (mentorApplication.Status != MentorApplicationStatus.Waiting.ToInt())
            {
                return new ErrorApiResponse(ResultMessage.ApplicationNotFoundWhenWaitingStatus);
            }

            bool checkMenteeCountGtOrEqual = MenteeCountOfMentorGtOrEqMaxCount(mentorId);

            if (checkMenteeCountGtOrEqual)
            {
                return new ErrorApiResponse(ResultMessage.MentorAlreadyHasTheMaxMenteeCount);
            }

            bool checkMentorCountGtOrEqual = MentorCountOfMenteeGtOrEqMaxCount(menteeId);

            if (checkMentorCountGtOrEqual)
            {
                return new ErrorApiResponse(ResultMessage.MenteeAlreadyHasTheMaxMentorCount);
            }

            DateTime now = DateTime.Now;

            mentorApplication.Status = MentorApplicationStatus.Approved.ToInt();
            mentorApplication.CompleteDate = now;

            applicationsRepository.Update(mentorApplication);

            var mentorMenteePairs = new MentorMenteePairs
            {
                MentorId = mentorId,
                MenteeId = menteeId,
                MentorStartDate = now,
                State = MentorMenteePairStatus.Continues.ToInt()
            };

            pairsRepository.Create(mentorMenteePairs);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        public async Task<ApiResponse> RejectMenteeAsync(int mentorUserId, int mentorId, int menteeId)
        {
            var mentor = await mentorRepository.GetByUserIdAsync(mentorUserId);

            if (mentor == null || mentor.Id != mentorId)
            {
                return new ErrorApiResponse(ResultMessage.UnAuthorized);
            }

            var mentorApplication = await applicationsRepository.GetAsync(mentorId, menteeId);

            if (mentorApplication == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentorMenteePair);
            }

            if (mentorApplication.Status != MentorApplicationStatus.Waiting.ToInt())
            {
                return new ErrorApiResponse(ResultMessage.ApplicationNotFoundWhenWaitingStatus);
            }

            mentorApplication.CompleteDate = DateTime.Now;
            mentorApplication.Status = MentorApplicationStatus.Rejected.ToInt();

            applicationsRepository.Update(mentorApplication);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        /// <summary>
        /// This method checks that the number of mentor of the mentee is greater than or equal to default max. value
        /// </summary>
        /// <param name="menteeId">mentee id</param>
        /// <returns>Number of mentor of the mentee is greater than or equal to default max. value?</returns>
        private bool MentorCountOfMenteeGtOrEqMaxCount(int menteeId)
        {
            int count = pairsRepository.GetCountForContinuesStatusByMenteeId(menteeId);
            return count >= devnotConfigurationContext.MaxMentorCountOfMentee;
        }

        /// <summary>
        /// This method checks that the number of mentee of the mentor is greater than or equal to default max. value
        /// </summary>
        /// <param name="mentorId">mentor id</param>
        /// <returns>Number of mentee of the mentor is greater than or equal to default max. value?</returns>
        private bool MenteeCountOfMentorGtOrEqMaxCount(int mentorId)
        {
            int count = pairsRepository.GetCountForContinuesStatusByMentorId(mentorId);
            return count >= devnotConfigurationContext.MaxMenteeCountOfMentor;
        }
    }
}

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
using System.Linq;
using System.Collections.Generic;

namespace DevnotMentor.Api.Services
{
    //[ExceptionHandlingAspect]
    public class MentorService : BaseService, IMentorService
    {
        private readonly  IMentorRepository mentorRepository;
        private readonly  IMenteeRepository menteeRepository;
        private readonly  IMentorLinksRepository mentorLinksRepository;
        private readonly  IMentorTagsRepository mentorTagsRepository;
        private readonly  ITagRepository tagRepository;
        private readonly IUserRepository userRepository;
        private readonly IMentorApplicationsRepository mentorApplicationsRepository;
        private readonly IMentorMenteePairsRepository mentorMenteePairsRepository;

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
            this.mentorApplicationsRepository = mentorApplicationsRepository;
            this.mentorMenteePairsRepository = mentorMenteePairsRepository;
        }

        public async Task<ApiResponse<MentorDto>> GetMentorProfile(string userName)
        {
            var user = await userRepository.GetByUserName(userName);

            if (user == null)
            {
                return new ErrorApiResponse<MentorDto>(data: default, ResultMessage.NotFoundUser);
            }

            var mentor = await mentorRepository.GetByUserId(user.Id);

            if (mentor == null)
            {
                return new ErrorApiResponse<MentorDto>(data: default, ResultMessage.NotFoundMentor);
            }

            var mappedMentor = mapper.Map<MentorDto>(mentor);
            return new SuccessApiResponse<MentorDto>(mappedMentor);
        }

        public async Task<ApiResponse<MentorDto>> CreateMentorProfile(CreateMentorProfileRequest request)
        {
            var user = await userRepository.GetById(request.UserId);

            if (user == null)
            {
                return new ErrorApiResponse<MentorDto>(data: default, message: ResultMessage.NotFoundUser);
            }

            var registeredMentor = await mentorRepository.GetByUserId(user.Id);

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

        public async Task<ApiResponse> AcceptMentee(int mentorUserId, int mentorId, int menteeId)
        {
            var mentor = await mentorRepository.GetByUserId(mentorUserId);

            if (mentor == null || mentor.Id != mentorId)
            {
                return new ErrorApiResponse(ResultMessage.UnAuthorized);
            }

            var mentorApplication = await mentorApplicationsRepository.Get(mentorId, menteeId);

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

            mentorApplicationsRepository.Update(mentorApplication);

            var mentorMenteePairs = new MentorMenteePairs
            {
                MentorId = mentorId,
                MenteeId = menteeId,
                MentorStartDate = now,
                State = MentorMenteePairStatus.Continues.ToInt()
            };

            mentorMenteePairsRepository.Create(mentorMenteePairs);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        public async Task<ApiResponse> RejectMentee(int mentorUserId, int mentorId, int menteeId)
        {
            var mentor = await mentorRepository.GetByUserId(mentorUserId);

            if (mentor == null || mentor.Id != mentorId)
            {
                return new ErrorApiResponse(ResultMessage.UnAuthorized);
            }

            var mentorApplication = await mentorApplicationsRepository.Get(mentorId, menteeId);

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

            mentorApplicationsRepository.Update(mentorApplication);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        public async Task<ApiResponse> GetMentees(string userName)
        {
            var user = await userRepository.GetByUserName(userName);

            if (user == null)
            {
                return new ErrorApiResponse<MentorDto>(data: default, ResultMessage.NotFoundUser);
            }

            var mentor = await mentorRepository.GetByUserId(user.Id);

            if (mentor == null)
            {
                return new ErrorApiResponse<MentorDto>(data: default, ResultMessage.NotFoundMentor);
            }
            
            var mappedData = mapper.Map<List<MenteeDto>>(await mentorRepository.GetMentees(x => x.Mentor.Id == mentor.Id));
            return new SuccessApiResponse<List<MenteeDto>>(mappedData);
        }
        /// <summary>
        /// This method checks that the number of mentor of the mentee is greater than or equal to default max. value
        /// </summary>
        /// <param name="menteeId">mentee id</param>
        /// <returns>Number of mentor of the mentee is greater than or equal to default max. value?</returns>
        private bool MentorCountOfMenteeGtOrEqMaxCount(int menteeId)
        {
            int count = mentorMenteePairsRepository.GetCountForContinuesStatusByMenteeId(menteeId);
            return count >= devnotConfigurationContext.MaxMentorCountOfMentee;
        }

        /// <summary>
        /// This method checks that the number of mentee of the mentor is greater than or equal to default max. value
        /// </summary>
        /// <param name="mentorId">mentor id</param>
        /// <returns>Number of mentee of the mentor is greater than or equal to default max. value?</returns>
        private bool MenteeCountOfMentorGtOrEqMaxCount(int mentorId)
        {
            int count = mentorMenteePairsRepository.GetCountForContinuesStatusByMentorId(mentorId);
            return count >= devnotConfigurationContext.MaxMenteeCountOfMentor;
        }
    }
}

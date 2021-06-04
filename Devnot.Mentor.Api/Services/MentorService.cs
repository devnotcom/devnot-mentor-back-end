using AutoMapper;
using DevnotMentor.Api.Aspects.Autofac.Exception;
using DevnotMentor.Api.Aspects.Autofac.UnitOfWork;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;

namespace DevnotMentor.Api.Services
{
    [ExceptionHandlingAspect]
    public class MentorService : BaseService, IMentorService
    {
        private IMentorRepository mentorRepository;
        private IMenteeRepository menteeRepository;
        private IMentorLinksRepository mentorLinksRepository;
        private IMentorTagsRepository mentorTagsRepository;
        private ITagRepository tagRepository;
        private IUserRepository userRepository;
        private IMentorApplicationsRepository mentorApplicationsRepository;
        private IMentorMenteePairsRepository mentorMenteePairsRepository;

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

        public async Task<ApiResponse<MentorProfileModel>> GetMentorProfile(string userName)
        {
            var response = new ApiResponse<MentorProfileModel>();

            var user = await userRepository.GetByUserName(userName);

            if (user != null)
            {
                var mentor = await mentorRepository.GetByUserId(user.Id);

                if (mentor != null)
                {
                    response.Data = mapper.Map<MentorProfileModel>(mentor);
                    response.Success = true;
                }
                else
                {
                    response.Message = ResultMessage.NotFoundMentor;
                }
            }
            else
            {
                response.Message = ResultMessage.NotFoundUser;
            }

            return response;
        }

        public async Task<ApiResponse<MentorProfileModel>> CreateMentorProfile(MentorProfileModel model)
        {
            var response = new ApiResponse<MentorProfileModel>();

            var user = await userRepository.GetByUserName(model.UserName);

            if (user != null)
            {
                var registeredMentor = await mentorRepository.GetByUserId(user.Id);

                if (registeredMentor == null)
                {
                    var mentor = await CreateNewMentor(model, user);

                    if (mentor != null)
                    {
                        response.Data = mapper.Map<MentorProfileModel>(mentor);
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = ResultMessage.UnhandledException;
                    }
                }
                else
                {
                    response.Message = ResultMessage.MentorAlreadyRegistered;
                }
            }
            else
            {

                response.Message = ResultMessage.NotFoundUser;
            }


            return response;
        }

        public Task<List<MentorProfileModel>> SearchMentor(SearchMentorModel model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMentorProfile(MentorProfileModel model)
        {
            throw new NotImplementedException();
        }

        [DevnotUnitOfWorkAspect]
        private async Task<Mentor> CreateNewMentor(MentorProfileModel model, User user)
        {
            Mentor mentor = null;

            var newMentor = mapper.Map<Mentor>(model);
            newMentor.UserId = user.Id;

            mentor = mentorRepository.Create(newMentor);
            if (mentor != null)
            {
                mentorLinksRepository.Create(mentor.Id, model.MentorLinks);

                foreach (var mentorTag in model.MentorTags)
                {
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
            }

            return mentor;
        }

        public async Task<ApiResponse> AcceptMentee(int mentorUserId, int menteeUserId)
        {
            var mentorApplication = await mentorApplicationsRepository.Get(mentorUserId, menteeUserId);

            if (mentorApplication == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentorMenteePair);
            }

            if (mentorApplication.Status == MentorMenteePairStatus.Approved.ToInt())
            {
                return new ErrorApiResponse(ResultMessage.ApplicationAlreadyApproved);
            }

            bool checkMenteeCountGtOrEqual = MenteeCountOfMentorGtOrEqMaxCount(mentorUserId);

            if (checkMenteeCountGtOrEqual)
            {
                return new ErrorApiResponse(ResultMessage.MentorAlreadyHasTheMaxMenteeCount);
            }

            bool checkMentorCountGtOrEqual = MentorCountOfMenteeGtOrEqMaxCount(menteeUserId);

            if (checkMentorCountGtOrEqual)
            {
                return new ErrorApiResponse(ResultMessage.MenteeAlreadyHasTheMaxMentorCount);
            }

            DateTime now = DateTime.Now;

            mentorApplication.Status = MentorMenteePairStatus.Approved.ToInt();
            mentorApplication.CompleteDate = now;

            mentorApplicationsRepository.Update(mentorApplication);

            int mentorId = await mentorRepository.GetIdByUserId(mentorUserId);
            int menteeId = await menteeRepository.GetIdByUserId(menteeUserId);

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

        public async Task<ApiResponse> RejectMentee(int mentorUserId, int menteeUserId)
        {
            var mentorApplication = await mentorApplicationsRepository.Get(mentorUserId, menteeUserId);

            if (mentorApplication == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentorMenteePair);
            }

            if (mentorApplication.Status != MentorMenteePairStatus.Waiting.ToInt())
            {
                return new ErrorApiResponse(ResultMessage.ApplicationNotFoundWhenWaitingStatus);
            }

            mentorApplication.Status = MentorMenteePairStatus.Rejected.ToInt();

            mentorApplicationsRepository.Update(mentorApplication);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        /// <summary>
        /// This method checks that the number of mentor of the mentee is greater than or equal to default max. value
        /// </summary>
        /// <param name="menteeUserId">user id of mentee</param>
        /// <returns>Number of mentor of the mentee is greater than or equal to default max. value?</returns>
        private bool MentorCountOfMenteeGtOrEqMaxCount(int menteeUserId)
        {
            int count = mentorMenteePairsRepository.GetCountForContinuesStatusByMenteeUserId(menteeUserId);
            return count >= devnotConfigurationContext.MaxMentorCountOfMentee;
        }

        /// <summary>
        /// This method checks that the number of mentee of the mentor is greater than or equal to default max. value
        /// </summary>
        /// <param name="mentorUserId">user id of mentor.</param>
        /// <returns>Number of mentee of the mentor is greater than or equal to default max. value?</returns>
        private bool MenteeCountOfMentorGtOrEqMaxCount(int mentorUserId)
        {
            int count = mentorMenteePairsRepository.GetCountForContinuesStatusByMentorUserId(mentorUserId);
            return count >= devnotConfigurationContext.MaxMenteeCountOfMentor;
        }
    }
}

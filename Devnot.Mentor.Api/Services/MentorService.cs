using AutoMapper;
using DevnotMentor.Api.Aspects.Autofac.Exception;
using DevnotMentor.Api.Aspects.Autofac.UnitOfWork;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

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
            IOptions<AppSettings> appSettings,
            IOptions<ResponseMessages> responseMessages,
            IMapper mapper,
            IMentorRepository mentorRepository,
            IMenteeRepository menteeRepository,
            IMentorLinksRepository mentorLinksRepository,
            IMentorTagsRepository mentorTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IMentorApplicationsRepository mentorApplicationsRepository,
            IMentorMenteePairsRepository mentorMenteePairsRepository,
            ILoggerRepository loggerRepository
            ) : base(appSettings, responseMessages, mapper, loggerRepository)
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
                    response.Message = responseMessages.Values["MentorNotFound"];
                }
            }
            else
            {
                response.Message = responseMessages.Values["UserNotFound"];
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
                        response.Message = responseMessages.Values["UnhandledException"];
                    }
                }
                else
                {
                    response.Message = responseMessages.Values["AlreadyRegisteredMentor"];
                }
            }
            else
            {
                response.Message = responseMessages.Values["UserNotFound"];
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
            var apiResponse = new ApiResponse();

            var mentorApplication = await mentorApplicationsRepository.Get(mentorUserId, menteeUserId);

            if (mentorApplication == null)
            {
                apiResponse.Message = responseMessages.Values["MentorMenteePairNotFound"];
                return apiResponse;
            }

            if (mentorApplication.Status == MentorMenteePairStatus.Approved.ToInt())
            {
                apiResponse.Message = responseMessages.Values["ApplicationAlreadyApproved"];
                return apiResponse;
            }

            bool checkMenteeCountGtOrEqual = MenteeCountOfMentorGtOrEqMaxCount(mentorUserId);

            if (checkMenteeCountGtOrEqual)
            {
                apiResponse.Message = responseMessages.Values["MentorAlreadyHasTheMaxMenteeCount"];
                return apiResponse;
            }

            bool checkMentorCountGtOrEqual = MentorCountOfMenteeGtOrEqMaxCount(menteeUserId);

            if (checkMentorCountGtOrEqual)
            {
                apiResponse.Message = responseMessages.Values["MenteeAlreadyHasTheMaxMentorCount"];
                return apiResponse;
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

            apiResponse.Success = true;
            return apiResponse;
        }

        public async Task<ApiResponse> RejectMentee(int mentorUserId, int menteeUserId)
        {
            var apiResponse = new ApiResponse();

            var mentorApplication = await mentorApplicationsRepository.Get(mentorUserId, menteeUserId);

            if (mentorApplication == null)
            {
                apiResponse.Message = responseMessages.Values["MentorMenteePairNotFound"];
                return apiResponse;
            }

            if (mentorApplication.Status != MentorMenteePairStatus.Waiting.ToInt())
            {
                apiResponse.Message = responseMessages.Values["ApplicationNotFoundWhenWaitingStatus"];
                return apiResponse;
            }

            mentorApplication.Status = MentorMenteePairStatus.Rejected.ToInt();

            mentorApplicationsRepository.Update(mentorApplication);

            apiResponse.Success = true;
            return apiResponse;

        }

        /// <summary>
        /// This method checks that the number of mentor of the mentee is greater than or equal to default max. value
        /// </summary>
        /// <param name="menteeUserId">user id of mentee</param>
        /// <returns>Number of mentor of the mentee is greater than or equal to default max. value?</returns>
        private bool MentorCountOfMenteeGtOrEqMaxCount(int menteeUserId)
        {
            int count = mentorMenteePairsRepository.GetCountForContinuesStatusByMenteeUserId(menteeUserId);
            return count >= appSettings.MaxMentorCountOfMentee;
        }

        /// <summary>
        /// This method checks that the number of mentee of the mentor is greater than or equal to default max. value
        /// </summary>
        /// <param name="mentorUserId">user id of mentor.</param>
        /// <returns>Number of mentee of the mentor is greater than or equal to default max. value?</returns>
        private bool MenteeCountOfMentorGtOrEqMaxCount(int mentorUserId)
        {
            int count = mentorMenteePairsRepository.GetCountForContinuesStatusByMentorUserId(mentorUserId);
            return count >= appSettings.MaxMenteeCountOfMentor;
        }
    }
}

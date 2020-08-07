using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories;
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
    public class MentorService : BaseService, IMentorService
    {
        MentorRepository mentorRepository;
        MenteeRepository menteeRepository;
        MentorLinksRepository mentorLinksRepository;
        MentorTagsRepository mentorTagsRepository;
        TagRepository tagRepository;
        UserRepository userRepository;
        MentorApplicationsRepository mentorApplicationsRepository;
        MentorMenteePairsRepository mentorMenteePairsRepository;

        public MentorService(IOptions<AppSettings> appSettings, IOptions<ResponseMessages> responseMessages, IMapper mapper, MentorDBContext context) : base(appSettings, responseMessages, mapper, context)
        {
            mentorRepository = new MentorRepository(context);
            mentorLinksRepository = new MentorLinksRepository(context);
            mentorTagsRepository = new MentorTagsRepository(context);
            tagRepository = new TagRepository(context);
            userRepository = new UserRepository(context);
            menteeRepository = new MenteeRepository(context);
            mentorApplicationsRepository = new MentorApplicationsRepository(context);
            mentorMenteePairsRepository = new MentorMenteePairsRepository(context);
        }

        public async Task<ApiResponse<MentorProfileModel>> GetMentorProfile(string userName)
        {
            var response = new ApiResponse<MentorProfileModel>();

            await RunInTry(response, async () =>
            {
                var user = userRepository.Filter(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    var mentor = mentorRepository.Filter(m => m.UserId == user.Id).FirstOrDefault();

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
            });

            return response;
        }

        public async Task<ApiResponse<MentorProfileModel>> CreateMentorProfile(MentorProfileModel model)
        {
            var response = new ApiResponse<MentorProfileModel>();

            await RunInTry(response, async () =>
            {

                var user = userRepository.Filter(u => u.UserName == model.UserName).FirstOrDefault();

                if (user != null)
                {
                    var isRegisteredMentor = mentorRepository.Filter(m => m.UserId == user.Id).Any();

                    if (!isRegisteredMentor)
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

            });

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

        private async Task<Mentor> CreateNewMentor(MentorProfileModel model, User user)
        {
            Mentor mentor = null;

            using (var transaction = new TransactionScope())
            {
                var newMentor = mapper.Map<Mentor>(model);
                newMentor.UserId = user.Id;

                mentor = mentorRepository.Create(newMentor);
                if (mentor != null)
                {
                    mentorLinksRepository.Create(mentor.Id, model.MentorLinks);

                    foreach (var mentorTag in model.MentorTags)
                    {
                        var tag = tagRepository.Filter(t => t.Name == mentorTag).FirstOrDefault();
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
                    await mentorRepository.SaveChangesAsync();
                    transaction.Complete();
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

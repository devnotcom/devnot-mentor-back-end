using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Controllers;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace DevnotMentor.Api.Services
{
    public class MenteeService : BaseService, IMenteeService
    {
        MenteeRepository menteeRepository;
        MenteeLinksRepository menteeLinksRepository;
        MenteeTagsRepository menteeTagsRepository;
        TagRepository tagRepository;
        UserRepository userRepository;
        MentorRepository mentorRepository;
        MentorApplicationsRepository mentorApplicationsRepository;

        public MenteeService(IOptions<AppSettings> appSettings, IOptions<ResponseMessages> responseMessages, IMapper mapper, MentorDBContext context) : base(appSettings, responseMessages, mapper, context)
        {
            // TODO: I will take fields with dependency injection when application successfully done :)

            this.mapper = mapper;
            menteeRepository = new MenteeRepository(context);
            menteeLinksRepository = new MenteeLinksRepository(context);
            menteeTagsRepository = new MenteeTagsRepository(context);
            tagRepository = new TagRepository(context);
            userRepository = new UserRepository(context);
            mentorRepository = new MentorRepository(context);
            mentorApplicationsRepository = new MentorApplicationsRepository(context);
        }

        public async Task<ApiResponse<MenteeProfileModel>> GetMenteeProfile(string userName)
        {
            var response = new ApiResponse<MenteeProfileModel>();

            await RunInTry(response, async () =>
            {
                var user = userRepository.Filter(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    var mentee = menteeRepository.Filter(m => m.UserId == user.Id).FirstOrDefault();

                    if (mentee != null)
                    {
                        response.Data = mapper.Map<MenteeProfileModel>(mentee);
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = responseMessages.Values["MenteeNotFound"];
                    }
                }
                else
                {
                    response.Message = responseMessages.Values["UserNotFound"];
                }
            });

            return response;
        }

        public async Task<ApiResponse<MenteeProfileModel>> CreateMenteeProfile(MenteeProfileModel model)
        {
            var response = new ApiResponse<MenteeProfileModel>();

            await RunInTry(response, async () =>
            {

                var user = userRepository.Filter(u => u.UserName == model.UserName).FirstOrDefault();

                if (user != null)
                {
                    var isRegisteredMentee = menteeRepository.Filter(m => m.UserId == user.Id).Any();

                    if (!isRegisteredMentee)
                    {
                        var mentee = await CreateNewMentee(model, user);

                        if (mentee != null)
                        {
                            response.Data = mapper.Map<MenteeProfileModel>(mentee);
                            response.Success = true;
                        }
                        else
                        {
                            response.Message = responseMessages.Values["UnhandledException"];
                        }
                    }
                    else
                    {
                        response.Message = responseMessages.Values["AlreadyRegisteredMentee"];
                    }
                }
                else
                {
                    response.Message = responseMessages.Values["UserNotFound"];
                }

            });

            return response;
        }




        //public void UpdateMenteeProfile(MenteeProfileModel model)
        //{
        //    throw new NotImplementedException();
        //}

        private async Task<Mentee> CreateNewMentee(MenteeProfileModel model, User user)
        {
            Mentee mentee = null;

            using (var transaction = new TransactionScope())
            {
                var newMentee = mapper.Map<Mentee>(model);
                newMentee.UserId = user.Id;

                mentee = menteeRepository.Create(newMentee);
                if (mentee != null)
                {
                    menteeLinksRepository.Create(mentee.Id, model.MenteeLinks);

                    foreach (var menteeTag in model.MenteeTags)
                    {
                        var tag = tagRepository.Filter(t => t.Name == menteeTag).FirstOrDefault();
                        if (tag != null)
                        {
                            menteeTagsRepository.Create(new MenteeTags { TagId = tag.Id, MenteeId = mentee.Id });
                        }
                        else
                        {
                            var newTag = tagRepository.Create(new Tag { Name = menteeTag });
                            if (newTag != null)
                            {
                                menteeTagsRepository.Create(new MenteeTags { TagId = newTag.Id, MenteeId = mentee.Id });
                            }
                        }
                    }
                    await menteeRepository.SaveChangesAsync();
                    transaction.Complete();
                }
            }

            return mentee;
        }

        public async Task<ApiResponse> ApplyToMentor(ApplyMentorModel model)
        {
            var apiResponse = new ApiResponse();

            if (model.MenteeUserId == model.MentorUserId)
            {
                apiResponse.Message = responseMessages.Values["MenteeCanNotBeSelfMentor"];
                return apiResponse;
            }

            var checkIsMenteeUserExists = await userRepository.AnyByIdAsync(model.MenteeUserId);
            var checkIsMentorUserExists = await userRepository.AnyByIdAsync(model.MentorUserId);

            if (!checkIsMenteeUserExists || !checkIsMentorUserExists)
            {
                apiResponse.Message = responseMessages.Values["UserNotFound"];
                return apiResponse;
            }

            bool checkIsMenteeExists = await menteeRepository.AnyByUserIdAsync(model.MenteeUserId);

            if (!checkIsMenteeExists)
            {
                apiResponse.Message = responseMessages.Values["MenteeNotFound"];
                return apiResponse;
            }

            bool checkIsMentorExists = await mentorRepository.AnyByUserIdAsync(model.MentorUserId);

            if (!checkIsMentorExists)
            {
                apiResponse.Message = responseMessages.Values["MentorNotFound"];
                return apiResponse;
            }

            bool checkAreThereExistsMenteeAndMentorPair = await mentorApplicationsRepository.AnyPairByUserIdAsync(model.MentorUserId, model.MenteeUserId);

            if (checkAreThereExistsMenteeAndMentorPair)
            {
                apiResponse.Message = responseMessages.Values["MentorMenteePairAlreadyExists"];
                return apiResponse;
            }

            int menteeId = await menteeRepository.GetIdByUserIdAsync(model.MenteeUserId);
            int mentorId = await mentorRepository.GetIdByUserIdAsync(model.MentorUserId);

            mentorApplicationsRepository.Create(new MentorApplications
            {
                ApllicationNotes = model.ApplicationNotes,
                ApplyDate = DateTime.Now,
                MenteeId = menteeId,
                MentorId = mentorId,
                Status = MentorMenteePairStatus.Waiting.ToInt()
            });

            apiResponse.Success = true;
            return apiResponse;
        }
    }
}

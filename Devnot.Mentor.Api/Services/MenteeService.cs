using AutoMapper;
using DevnotMentor.Api.Aspects.Autofac.Exception;
using DevnotMentor.Api.Aspects.Autofac.UnitOfWork;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Controllers;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Helpers.Extensions;
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
    public class MenteeService : IMenteeService
    {
        MenteeRepository menteeRepository;
        MenteeLinksRepository menteeLinksRepository;
        MenteeTagsRepository menteeTagsRepository;
        TagRepository tagRepository;
        UserRepository userRepository;
        MentorRepository mentorRepository;
        MentorApplicationsRepository mentorApplicationsRepository;

        ResponseMessages responseMessages;
        IMapper mapper;

        public MenteeService(IOptions<ResponseMessages> responseMessages, IMapper mapper, MentorDBContext context)
        {
            // TODO: I will take fields with dependency injection when application successfully done :)

            menteeRepository = new MenteeRepository(context);
            menteeLinksRepository = new MenteeLinksRepository(context);
            menteeTagsRepository = new MenteeTagsRepository(context);
            tagRepository = new TagRepository(context);
            userRepository = new UserRepository(context);
            mentorRepository = new MentorRepository(context);
            mentorApplicationsRepository = new MentorApplicationsRepository(context);

            this.responseMessages = responseMessages.Value;
            this.mapper = mapper;
        }

        [ExceptionHandlingAspect]
        public ApiResponse<MenteeProfileModel> GetMenteeProfile(string userName)
        {
            var response = new ApiResponse<MenteeProfileModel>();

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

            return response;
        }

        [DevnotUnitOfWorkAspect]
        [ExceptionHandlingAspect]
        public async Task<ApiResponse<MenteeProfileModel>> CreateMenteeProfile(MenteeProfileModel model)
        {
            var response = new ApiResponse<MenteeProfileModel>();

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

            return response;
        }

        [ExceptionHandlingAspect]
        [DevnotUnitOfWorkAspect]
        private async Task<Mentee> CreateNewMentee(MenteeProfileModel model, User user)
        {
            Mentee mentee = null;

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
            }

            return mentee;
        }

        [ExceptionHandlingAspect]
        public async Task<ApiResponse> ApplyToMentor(ApplyMentorModel model)
        {
            var apiResponse = new ApiResponse();

            if (model.MenteeUserId == model.MentorUserId)
            {
                apiResponse.Message = responseMessages.Values["MenteeCanNotBeSelfMentor"];
                return apiResponse;
            }

            int menteeId = await menteeRepository.GetIdByUserId(model.MenteeUserId);

            if (menteeId == default)
            {
                apiResponse.Message = responseMessages.Values["MenteeNotFound"];
                return apiResponse;
            }

            int mentorId = await mentorRepository.GetIdByUserId(model.MentorUserId);

            if (mentorId == default)
            {
                apiResponse.Message = responseMessages.Values["MentorNotFound"];
                return apiResponse;
            }

            bool checkAreThereExistsMenteeAndMentorPair = await mentorApplicationsRepository.IsExistsByUserId(model.MentorUserId, model.MenteeUserId);

            if (checkAreThereExistsMenteeAndMentorPair)
            {
                apiResponse.Message = responseMessages.Values["MentorMenteePairAlreadyExists"];
                return apiResponse;
            }

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

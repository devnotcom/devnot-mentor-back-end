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
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace DevnotMentor.Api.Services
{
    [ExceptionHandlingAspect]
    public class MenteeService : BaseService, IMenteeService
    {
        private IMenteeRepository menteeRepository;
        private IMenteeLinksRepository menteeLinksRepository;
        private IMenteeTagsRepository menteeTagsRepository;
        private ITagRepository tagRepository;
        private IUserRepository userRepository;
        private IMentorRepository mentorRepository;
        private IMentorApplicationsRepository mentorApplicationsRepository;

        public MenteeService(
            IOptions<AppSettings> appSettings,
            IOptions<ResponseMessages> responseMessages,
            IMapper mapper,
            IMenteeRepository menteeRepository,
            IMenteeLinksRepository menteeLinksRepository,
            IMenteeTagsRepository menteeTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IMentorRepository mentorRepository,
            IMentorApplicationsRepository mentorApplicationsRepository,
            ILoggerRepository loggerRepository
            )
            : base(appSettings, responseMessages, mapper, loggerRepository)
        {
            this.menteeRepository = menteeRepository;
            this.menteeLinksRepository = menteeLinksRepository;
            this.menteeTagsRepository = menteeTagsRepository;
            this.tagRepository = tagRepository;
            this.userRepository = userRepository;
            this.mentorRepository = mentorRepository;
            this.mentorApplicationsRepository = mentorApplicationsRepository;
        }

        public async Task<ApiResponse<MenteeProfileModel>> GetMenteeProfile(string userName)
        {
            var response = new ApiResponse<MenteeProfileModel>();

            var user = await userRepository.GetByUserName(userName);

            if (user != null)
            {
                var mentee = await menteeRepository.GetByUserId(user.Id);

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

        //[DevnotUnitOfWorkAspect]
        public async Task<ApiResponse<MenteeProfileModel>> CreateMenteeProfile(MenteeProfileModel model)
        {
            var response = new ApiResponse<MenteeProfileModel>();

            var user = await userRepository.GetByUserName(model.UserName);

            if (user != null)
            {
                var registeredMentee = await menteeRepository.GetByUserId(user.Id);

                if (registeredMentee == null)
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
                    var tag = tagRepository.Get(menteeTag);

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

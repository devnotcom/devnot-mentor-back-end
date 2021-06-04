using AutoMapper;
using DevnotMentor.Api.Aspects.Autofac.Exception;
using DevnotMentor.Api.Aspects.Autofac.UnitOfWork;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;

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
            IMapper mapper,
            IMenteeRepository menteeRepository,
            IMenteeLinksRepository menteeLinksRepository,
            IMenteeTagsRepository menteeTagsRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IMentorRepository mentorRepository,
            IMentorApplicationsRepository mentorApplicationsRepository,
            ILoggerRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext
            )
            : base(mapper, loggerRepository, devnotConfigurationContext)
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
                    response.Message = ResultMessage.NotFoundMentee;
                }
            }
            else
            {
                response.Message = ResultMessage.NotFoundUser;
            }

            return response;
        }

        [DevnotUnitOfWorkAspect]
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
                        response.Message = ResultMessage.UnhandledException;
                    }
                }
                else
                {
                    response.Message = ResultMessage.MenteeAlreadyRegistered;
                }
            }
            else
            {
                response.Message = ResultMessage.NotFoundUser;
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
                return new ErrorApiResponse(ResultMessage.MenteeCanNotBeSelfMentor);
            }

            int menteeId = await menteeRepository.GetIdByUserId(model.MenteeUserId);

            if (menteeId == default)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentee);
            }

            int mentorId = await mentorRepository.GetIdByUserId(model.MentorUserId);

            if (mentorId == default)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentor);
            }

            bool checkAreThereExistsMenteeAndMentorPair = await mentorApplicationsRepository.IsExistsByUserId(model.MentorUserId, model.MenteeUserId);

            if (checkAreThereExistsMenteeAndMentorPair)
            {
                return new ErrorApiResponse(ResultMessage.MentorMenteePairAlreadyExist);
            }

            mentorApplicationsRepository.Create(new MentorApplications
            {
                ApllicationNotes = model.ApplicationNotes,
                ApplyDate = DateTime.Now,
                MenteeId = menteeId,
                MentorId = mentorId,
                Status = MentorMenteePairStatus.Waiting.ToInt()
            });

            return new SuccessApiResponse();
        }
    }
}

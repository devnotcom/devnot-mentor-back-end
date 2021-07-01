using AutoMapper;
using DevnotMentor.Api.Aspects.Autofac.Exception;
using DevnotMentor.Api.Aspects.Autofac.UnitOfWork;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;

namespace DevnotMentor.Api.Services
{
    //[ExceptionHandlingAspect]
    public class MenteeService : BaseService, IMenteeService
    {
        private readonly IMenteeRepository menteeRepository;
        private readonly  IMenteeLinksRepository menteeLinksRepository;
        private readonly  IMenteeTagsRepository menteeTagsRepository;
        private readonly  ITagRepository tagRepository;
        private readonly  IUserRepository userRepository;
        private readonly  IMentorRepository mentorRepository;
        private readonly  IMentorApplicationsRepository mentorApplicationsRepository;

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

        public async Task<ApiResponse<MenteeDto>> GetMenteeProfile(string userName)
        {
            var user = await userRepository.GetByUserName(userName);

            if (user == null)
            {
                return new ErrorApiResponse<MenteeDto>(data: default, message: ResultMessage.NotFoundUser);
            }

            var mentee = await menteeRepository.GetByUserId(user.Id);

            if (mentee == null)
            {
                return new ErrorApiResponse<MenteeDto>(data: default, message: ResultMessage.NotFoundMentee);
            }

            var mappedMentee = mapper.Map<Mentee, MenteeDto>(mentee);
            return new SuccessApiResponse<MenteeDto>(mappedMentee);
        }

        //[DevnotUnitOfWorkAspect]
        public async Task<ApiResponse<MenteeDto>> CreateMenteeProfile(CreateMenteeProfileRequest request)
        {
            var user = await userRepository.GetById(request.UserId);

            if (user == null)
            {
                return new ErrorApiResponse<MenteeDto>(data: default, message: ResultMessage.NotFoundUser);
            }

            var registeredMentee = await menteeRepository.GetByUserId(user.Id);

            if (registeredMentee != null)
            {
                return new ErrorApiResponse<MenteeDto>(data: default, message: ResultMessage.MenteeAlreadyRegistered);
            }

            var mentee = await CreateNewMentee(request, user);

            if (mentee == null)
            {
                return new ErrorApiResponse<MenteeDto>(data: default, ResultMessage.FailedToAddMentee);
            }

            var mappedMentee = mapper.Map<MenteeDto>(mentee);
            return new SuccessApiResponse<MenteeDto>(mappedMentee);
        }

        private async Task<Mentee> CreateNewMentee(CreateMenteeProfileRequest request, User user)
        {
            Mentee mentee = null;

            var newMentee = mapper.Map<Mentee>(request);
            newMentee.UserId = user.Id;

            mentee = menteeRepository.Create(newMentee);

            if (mentee == null)
            {
                return null;
            }

            menteeLinksRepository.Create(mentee.Id, request.MenteeLinks);

            foreach (var menteeTag in request.MenteeTags)
            {
                if (String.IsNullOrWhiteSpace(menteeTag))
                {
                    continue;
                }

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

            return mentee;
        }

        public async Task<ApiResponse> ApplyToMentor(ApplyToMentorRequest request)
        {
            if (request.MenteeUserId == request.MentorUserId)
            {
                return new ErrorApiResponse(ResultMessage.MenteeCanNotBeSelfMentor);
            }

            int menteeId = await menteeRepository.GetIdByUserId(request.MenteeUserId);

            if (menteeId == default)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentee);
            }

            int mentorId = await mentorRepository.GetIdByUserId(request.MentorUserId);

            if (mentorId == default)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentor);
            }

            bool checkAreThereExistsMenteeAndMentorPair = await mentorApplicationsRepository.IsExistsByUserId(request.MentorUserId, request.MenteeUserId);

            if (checkAreThereExistsMenteeAndMentorPair)
            {
                return new ErrorApiResponse(ResultMessage.MentorMenteePairAlreadyExist);
            }

            mentorApplicationsRepository.Create(new MentorApplications
            {
                ApllicationNotes = request.ApplicationNotes,
                ApplyDate = DateTime.Now,
                MenteeId = menteeId,
                MentorId = mentorId,
                Status = MentorApplicationStatus.Waiting.ToInt()
            });

            return new SuccessApiResponse(ResultMessage.Success);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;

namespace DevnotMentor.Api.Services
{
    public class ApplicationService : BaseService, IApplicationService
    {

        private readonly IMentorApplicationsRepository applicationsRepository;
        private readonly IMentorMenteePairsRepository pairRepository;
        private readonly IMentorRepository mentorRepository;
        private readonly IMenteeRepository menteeRepository;

        public ApplicationService(
            IMapper mapper,
            ILoggerRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext,
            IMentorApplicationsRepository mentorApplicationsRepository,
            IMentorMenteePairsRepository mentorMenteePairsRepository,
            IMentorRepository mentorRepository,
            IMenteeRepository menteeRepository
        ) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            this.applicationsRepository = mentorApplicationsRepository;
            this.pairRepository = mentorMenteePairsRepository;
            this.mentorRepository = mentorRepository;
            this.menteeRepository = menteeRepository;
        }

        public async Task<ApiResponse<List<MentorApplicationsDto>>> GetApplicationsByUserIdAsync(int authorizedUserId)
        {
            var applications = await applicationsRepository.GetApplicationsByUserIdAsync(authorizedUserId);
            var applicationsDto = mapper.Map<List<MentorApplicationsDto>>(applications);

            return new SuccessApiResponse<List<MentorApplicationsDto>>(applicationsDto);
        }

        public async Task<ApiResponse> AcceptApplicationByIdAsync(int authorizedUserId, int applicationId)
        {
            var application = await applicationsRepository.GetWhichIsWaitingByIdAsync(applicationId);
            if (application == null)
            {
                return new ErrorApiResponse(ResultMessage.ApplicationNotFoundWhenWaitingStatus);
            }

            bool isUserMentorForThePair = application.Mentor.UserId == authorizedUserId;
            if (isUserMentorForThePair == false)
            {
                return new ErrorApiResponse(ResultMessage.Forbidden);
            }

            if (IsMentorshipsWhichAreContinuingCountGreaterThanOREqualTheMaxCountForMentee((int)application.MenteeId))
            {
                return new ErrorApiResponse(ResultMessage.MenteeAlreadyHasTheMaxMentorCount);
            }

            if (IsMentorshipsWhichAreContinuingCountGreaterThanOREqualTheMaxCountForMentor((int)application.MentorId))
            {
                return new ErrorApiResponse(ResultMessage.MentorAlreadyHasTheMaxMenteeCount);
            }

            var dateTimeNow = System.DateTime.Now;

            var mentorMenteePairs = new MentorMenteePairs
            {
                MentorId = application.MentorId,
                MenteeId = application.MenteeId,
                MentorStartDate = dateTimeNow,
                State = MentorMenteePairStatus.Continues.ToInt()
            };
            pairRepository.Create(mentorMenteePairs);

            application.Status = MentorApplicationStatus.Approved.ToInt();
            application.CompleteDate = dateTimeNow;
            applicationsRepository.Update(application);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        public async Task<ApiResponse> RejectApplicationByIdAsync(int authorizedUserId, int applicationId)
        {
            var application = await applicationsRepository.GetWhichIsWaitingByIdAsync(applicationId);
            if (application == null)
            {
                return new ErrorApiResponse(ResultMessage.ApplicationNotFoundWhenWaitingStatus);
            }

            bool isUserMentorForThePair = application.Mentor.UserId == authorizedUserId;
            if (isUserMentorForThePair == false)
            {
                return new ErrorApiResponse(ResultMessage.Forbidden);
            }

            application.Status = MentorApplicationStatus.Rejected.ToInt();
            application.CompleteDate = System.DateTime.Now;
            applicationsRepository.Update(application);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        private bool IsMentorshipsWhichAreContinuingCountGreaterThanOREqualTheMaxCountForMentee(int menteeId)
        {
            int count = pairRepository.GetCountForContinuingStatusByMenteeId(menteeId);
            return count >= devnotConfigurationContext.MaxMentorCountOfMentee;
        }

        private bool IsMentorshipsWhichAreContinuingCountGreaterThanOREqualTheMaxCountForMentor(int mentorId)
        {
            int count = pairRepository.GetCountForContinuingStatusByMentorId(mentorId);
            return count >= devnotConfigurationContext.MaxMenteeCountOfMentor;
        }

        public async Task<ApiResponse> CreateApplicationAsync(ApplicationRequest request)
        {
            if (request.MenteeUserId == request.MentorUserId)
            {
                return new ErrorApiResponse(ResultMessage.MenteeCanNotBeSelfMentor);
            }

            int menteeId = await menteeRepository.GetIdByUserIdAsync(request.MenteeUserId);

            if (menteeId == default)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentee);
            }

            int mentorId = await mentorRepository.GetIdByUserIdAsync(request.MentorUserId);

            if (mentorId == default)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentor);
            }

            if (await applicationsRepository.AnyWaitingApplicationBetweenMentorAndMenteeAsync(mentorId, menteeId))
            {
                return new ErrorApiResponse(ResultMessage.ThereIsAlreadyWaitingApplication);
            }

            applicationsRepository.Create(new MentorApplications
            {
                ApllicationNotes = request.ApplicationNotes,
                ApplyDate = System.DateTime.Now,
                MenteeId = menteeId,
                MentorId = mentorId,
                Status = MentorApplicationStatus.Waiting.ToInt()
            });

            return new SuccessApiResponse(ResultMessage.Success);
        }
    }
}
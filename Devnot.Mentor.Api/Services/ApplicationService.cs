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
        private readonly IMentorApplicationsRepository applicationRepository;
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
            this.applicationRepository = mentorApplicationsRepository;
            this.pairRepository = mentorMenteePairsRepository;
            this.mentorRepository = mentorRepository;
            this.menteeRepository = menteeRepository;
        }

        public async Task<ApiResponse<List<MentorApplicationsDto>>> GetApplicationsByUserIdAsync(int authenticatedUserId)
        {
            var applications = await applicationRepository.GetApplicationsByUserIdAsync(authenticatedUserId);
            var applicationsDto = mapper.Map<List<MentorApplicationsDto>>(applications);

            return new SuccessApiResponse<List<MentorApplicationsDto>>(applicationsDto);
        }

        public async Task<ApiResponse> ApproveWaitingApplicationByIdAsync(int authenticatedUserId, int toBeApprovedApplicationId)
        {
            var toBeApprovedApplication = await applicationRepository.GetWhichIsWaitingByIdAsync(toBeApprovedApplicationId);
            if (toBeApprovedApplication == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundWaitingApplication);
            }

            bool userMentorOfApplication = toBeApprovedApplication.Mentor.UserId == authenticatedUserId;
            if (!userMentorOfApplication)
            {
                return new ErrorApiResponse(ResultMessage.Forbidden);
            }

            if (isCountOfContinuingMentorshipsGreaterThanOREqualToMaxCountForMentee((int)toBeApprovedApplication.MenteeId))
            {
                return new ErrorApiResponse(ResultMessage.MenteeAlreadyHasTheMaxMentorCount);
            }

            if (isCountOfContinuingMentorshipsGreaterThanOREqualToMaxCountForMentor((int)toBeApprovedApplication.MentorId))
            {
                return new ErrorApiResponse(ResultMessage.MentorAlreadyHasTheMaxMenteeCount);
            }

            var dateTimeNow = System.DateTime.Now;

            var mentorMenteePairs = new MentorMenteePairs
            {
                MentorId = toBeApprovedApplication.MentorId,
                MenteeId = toBeApprovedApplication.MenteeId,
                MentorStartDate = dateTimeNow,
                State = MentorMenteePairStatus.Continues.ToInt()
            };
            pairRepository.Create(mentorMenteePairs);

            toBeApprovedApplication.Status = MentorApplicationStatus.Approved.ToInt();
            toBeApprovedApplication.CompleteDate = dateTimeNow;
            applicationRepository.Update(toBeApprovedApplication);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        public async Task<ApiResponse> RejectWaitingApplicationByIdAsync(int authenticatedUserId, int toBeRejectedApplicationId)
        {
            var toBeRejectedApplication = await applicationRepository.GetWhichIsWaitingByIdAsync(toBeRejectedApplicationId);
            if (toBeRejectedApplication == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundWaitingApplication);
            }

            bool userMentorOfApplication = toBeRejectedApplication.Mentor.UserId == authenticatedUserId;
            if (!userMentorOfApplication)
            {
                return new ErrorApiResponse(ResultMessage.Forbidden);
            }

            toBeRejectedApplication.Status = MentorApplicationStatus.Rejected.ToInt();
            toBeRejectedApplication.CompleteDate = System.DateTime.Now;
            applicationRepository.Update(toBeRejectedApplication);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        private bool isCountOfContinuingMentorshipsGreaterThanOREqualToMaxCountForMentee(int menteeId)
        {
            int count = pairRepository.GetCountForContinuingStatusByMenteeId(menteeId);
            return count >= devnotConfigurationContext.MaxMentorCountOfMentee;
        }

        private bool isCountOfContinuingMentorshipsGreaterThanOREqualToMaxCountForMentor(int mentorId)
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

            bool anyWaitingApplicationBetweenMentorAndMentee = await applicationRepository.AnyWaitingApplicationBetweenMentorAndMenteeAsync(mentorId, menteeId);
            if (anyWaitingApplicationBetweenMentorAndMentee)
            {
                return new ErrorApiResponse(ResultMessage.ThereIsAlreadyWaitingApplication);
            }

            applicationRepository.Create(new MentorApplications
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
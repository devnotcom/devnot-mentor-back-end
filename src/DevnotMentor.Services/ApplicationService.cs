using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevnotMentor.Common;
using DevnotMentor.Common.API;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentee;
using DevnotMentor.Data.Entities;
using DevnotMentor.Common.Enums;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Services.Interfaces;
using DevnotMentor.Utilities.Email;

namespace DevnotMentor.Services
{
    public class ApplicationService : BaseService, IApplicationService
    {
        private readonly IApplicationsRepository applicationRepository;
        private readonly IMentorshipsRepository pairRepository;
        private readonly IMentorRepository mentorRepository;
        private readonly IMenteeRepository menteeRepository;
        private readonly IUserRepository userRepository;
        private readonly IMailService mailService;

        public ApplicationService(
            IMapper mapper,
            ILogRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext,
            IApplicationsRepository mentorApplicationsRepository,
            IMentorshipsRepository MentorshipsRepository,
            IMentorRepository mentorRepository,
            IMenteeRepository menteeRepository,
            IUserRepository userRepository,
            IMailService mailService
        ) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            this.applicationRepository = mentorApplicationsRepository;
            this.pairRepository = MentorshipsRepository;
            this.mentorRepository = mentorRepository;
            this.menteeRepository = menteeRepository;
            this.userRepository = userRepository;
            this.mailService = mailService;
        }

        public async Task<ApiResponse<List<ApplicationDTO>>> GetApplicationsByUserIdAsync(int authenticatedUserId)
        {
            var applications = await applicationRepository.GetApplicationsByUserIdAsync(authenticatedUserId);
            var ApplicationDTO = mapper.Map<List<ApplicationDTO>>(applications);

            return new SuccessApiResponse<List<ApplicationDTO>>(ApplicationDTO);
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

            var Mentorships = new Mentorship
            {
                MentorId = toBeApprovedApplication.MentorId,
                MenteeId = toBeApprovedApplication.MenteeId,
                MentorStartDate = dateTimeNow,
                State = (int)MentorshipStatus.Continues
            };
            pairRepository.Create(Mentorships);

            toBeApprovedApplication.Status = (int)ApplicationStatus.Approved;
            toBeApprovedApplication.CompletedAt = dateTimeNow;
            applicationRepository.Update(toBeApprovedApplication);

            var to = new List<string>() { toBeApprovedApplication.Mentee.User.Email };
            await mailService.SendEmailAsync(
                to,
                EmailTemplate.ApplicationSubject,
                EmailTemplate.ApplicationApprovedBody(toBeApprovedApplication.Mentor.User, toBeApprovedApplication.Mentee.User));

            return new SuccessApiResponse();
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

            toBeRejectedApplication.Status = (int)ApplicationStatus.Rejected;
            toBeRejectedApplication.CompletedAt = System.DateTime.Now;
            applicationRepository.Update(toBeRejectedApplication);

            return new SuccessApiResponse();
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

            applicationRepository.Create(new Application
            {
                Note = request.ApplicationNotes,
                AppliedAt = System.DateTime.Now,
                MenteeId = menteeId,
                MentorId = mentorId,
                Status = (int)ApplicationStatus.Waiting
            });

            var mentorUser = await userRepository.GetByIdAsync(request.MentorUserId);
            var menteeUser = await userRepository.GetByIdAsync(request.MenteeUserId);

            var to = new List<string>() { mentorUser.Email };
            await mailService.SendEmailAsync(
                to,
                EmailTemplate.ApplicationSubject,
                EmailTemplate.ApplicationAppliedBody(mentorUser, menteeUser));

            return new SuccessApiResponse(ResponseStatus.Created);
        }
    }
}
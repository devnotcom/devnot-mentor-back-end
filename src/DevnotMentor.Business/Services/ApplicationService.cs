using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevnotMentor.Common.API;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentee;
using DevnotMentor.Data.Entities;
using DevnotMentor.Common.Enums;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Business.Services.Interfaces;
using DevnotMentor.Business.Utilities.Email;

namespace DevnotMentor.Business.Services
{
    public class ApplicationService : BaseService, IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMentorshipRepository _mentorshipRepository;
        private readonly IMentorRepository _mentorRepository;
        private readonly IMenteeRepository _menteeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        public ApplicationService(
            IMapper mapper,
            ILogRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext,
            IApplicationRepository mentorApplicationsRepository,
            IMentorshipRepository MentorshipsRepository,
            IMentorRepository mentorRepository,
            IMenteeRepository menteeRepository,
            IUserRepository userRepository,
            IMailService mailService
        ) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            _applicationRepository = mentorApplicationsRepository;
            _mentorshipRepository = MentorshipsRepository;
            _mentorRepository = mentorRepository;
            _menteeRepository = menteeRepository;
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public async Task<ApiResponse<List<ApplicationDTO>>> GetApplicationsByUserIdAsync(int authenticatedUserId)
        {
            var applications = await _applicationRepository.GetApplicationsByUserIdAsync(authenticatedUserId);
            var mappedApplications = _mapper.Map<List<ApplicationDTO>>(applications);

            return new SuccessApiResponse<List<ApplicationDTO>>(mappedApplications);
        }

        public async Task<ApiResponse> ApproveWaitingApplicationByIdAsync(int authenticatedUserId, int toBeApprovedApplicationId)
        {
            var toBeApprovedApplication = await _applicationRepository.GetWhichIsWaitingByIdAsync(toBeApprovedApplicationId);
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

            var mentorship = new Mentorship
            {
                MentorId = toBeApprovedApplication.MentorId,
                MenteeId = toBeApprovedApplication.MenteeId,
                StartedAt = dateTimeNow,
                State = (int)MentorshipStatus.Continues
            };
            _mentorshipRepository.Create(mentorship);

            toBeApprovedApplication.Status = (int)ApplicationStatus.Approved;
            toBeApprovedApplication.CompletedAt = dateTimeNow;
            _applicationRepository.Update(toBeApprovedApplication);

            var to = new List<string>() { toBeApprovedApplication.Mentee.User.Email };
            await _mailService.SendEmailAsync(
                to,
                EmailTemplate.ApplicationSubject,
                EmailTemplate.ApplicationApprovedBody(toBeApprovedApplication.Mentor.User, toBeApprovedApplication.Mentee.User));

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse> RejectWaitingApplicationByIdAsync(int authenticatedUserId, int toBeRejectedApplicationId)
        {
            var toBeRejectedApplication = await _applicationRepository.GetWhichIsWaitingByIdAsync(toBeRejectedApplicationId);
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
            _applicationRepository.Update(toBeRejectedApplication);

            return new SuccessApiResponse();
        }

        private bool isCountOfContinuingMentorshipsGreaterThanOREqualToMaxCountForMentee(int menteeId)
        {
            int count = _mentorshipRepository.GetCountForContinuingStatusByMenteeId(menteeId);
            return count >= _devnotConfigurationContext.MaxMentorCountOfMentee;
        }

        private bool isCountOfContinuingMentorshipsGreaterThanOREqualToMaxCountForMentor(int mentorId)
        {
            int count = _mentorshipRepository.GetCountForContinuingStatusByMentorId(mentorId);
            return count >= _devnotConfigurationContext.MaxMenteeCountOfMentor;
        }

        public async Task<ApiResponse<ApplicationDTO>> CreateApplicationAsync(ApplicationRequest request)
        {
            if (request.MenteeUserId == request.MentorUserId)
            {
                return new ErrorApiResponse<ApplicationDTO>(ResponseStatus.BadRequest, default, ResultMessage.MenteeCanNotBeSelfMentor);
            }

            int menteeId = await _menteeRepository.GetIdByUserIdAsync(request.MenteeUserId);
            if (menteeId == default)
            {
                return new ErrorApiResponse<ApplicationDTO>(ResponseStatus.NotFound, default, ResultMessage.NotFoundMentee);
            }

            int mentorId = await _mentorRepository.GetIdByUserIdAsync(request.MentorUserId);
            if (mentorId == default)
            {
                return new ErrorApiResponse<ApplicationDTO>(ResponseStatus.NotFound, default, ResultMessage.NotFoundMentor);
            }

            bool anyWaitingApplicationBetweenMentorAndMentee = await _applicationRepository.AnyWaitingApplicationBetweenMentorAndMenteeAsync(mentorId, menteeId);
            if (anyWaitingApplicationBetweenMentorAndMentee)
            {
                return new ErrorApiResponse<ApplicationDTO>(ResponseStatus.BadRequest, default, ResultMessage.ThereIsAlreadyWaitingApplication);
            }

            var mappedNewApplication = _mapper.Map<ApplicationDTO>(
                _applicationRepository.Create(new Application
                {
                    Note = request.ApplicationNotes,
                    AppliedAt = System.DateTime.Now,
                    MenteeId = menteeId,
                    MentorId = mentorId,
                    Status = (int)ApplicationStatus.Waiting
                })
            );

            var mentorUser = await _userRepository.GetByIdAsync(request.MentorUserId);
            var menteeUser = await _userRepository.GetByIdAsync(request.MenteeUserId);

            var to = new List<string>() { mentorUser.Email };
            await _mailService.SendEmailAsync(
                to,
                EmailTemplate.ApplicationSubject,
                EmailTemplate.ApplicationAppliedBody(mentorUser, menteeUser));

            return new SuccessApiResponse<ApplicationDTO>(ResponseStatus.Created, mappedNewApplication);
        }
    }
}
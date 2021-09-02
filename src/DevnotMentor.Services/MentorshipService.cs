using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevnotMentor.Common.API;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentorship;
using DevnotMentor.Data.Entities;
using DevnotMentor.Common.Enums;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Services.Interfaces;

namespace DevnotMentor.Services
{
    public class MentorshipService : BaseService, IMentorshipService
    {
        private readonly IMentorshipRepository _mentorshipRepository;
        private readonly IMenteeRepository _menteeRepository;
        private readonly IMentorRepository _mentorRepository;

        public MentorshipService(IMapper mapper,
                            IMentorshipRepository mentorshipRepository,
                            IMenteeRepository menteeRepository,
                            IMentorRepository mentorRepository,
                            ILogRepository logger,
                            IDevnotConfigurationContext devnotConfigurationContext
                        ) : base(mapper, logger, devnotConfigurationContext)
        {
            _mentorshipRepository = mentorshipRepository;
            _menteeRepository = menteeRepository;
            _mentorRepository = mentorRepository;
        }

        public async Task<ApiResponse<List<MentorshipDTO>>> GetMentorshipsOfMenteeByUserId(int userId)
        {
            var mentee = await _menteeRepository.GetByUserIdAsync(userId);
            if (mentee == null)
            {
                return new ErrorApiResponse<List<MentorshipDTO>>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentee);
            }

            var pairs = mapper.Map<List<MentorshipDTO>>(await _mentorshipRepository.GetMentorshipsByUserIdAsync(userId));

            return new SuccessApiResponse<List<MentorshipDTO>>(pairs);
        }

        public async Task<ApiResponse<List<MentorshipDTO>>> GetMentorshipsOfMentorByUserIdAsync(int userId)
        {
            var mentor = await _mentorRepository.GetByUserIdAsync(userId);
            if (mentor == null)
            {
                return new ErrorApiResponse<List<MentorshipDTO>>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentor);
            }

            var pairs = mapper.Map<List<MentorshipDTO>>(await _mentorshipRepository.GetMentorshipsByUserIdAsync(userId));

            return new SuccessApiResponse<List<MentorshipDTO>>(pairs);
        }

        public async Task<ApiResponse> FinishContinuingMentorshipAsync(int userId, int mentorshipId)
        {
            var toBeFinishedMentorship = await _mentorshipRepository.GetWhichIsNotFinishedYetByIdAsync(mentorshipId);
            if (toBeFinishedMentorship == null)
            {
                return new ErrorApiResponse(ResponseStatus.NotFound, ResultMessage.NotFoundNotFinishedMentorMenteePair);
            }

            bool userRelatedToPair = toBeFinishedMentorship.Mentee.UserId == userId || toBeFinishedMentorship.Mentor.UserId == userId;
            if (!userRelatedToPair)
            {
                return new ErrorApiResponse(ResponseStatus.Forbid, ResultMessage.Forbidden);
            }

            toBeFinishedMentorship.State = (int)MentorshipStatus.Finished;
            toBeFinishedMentorship.FinishedAt = System.DateTime.Now;

            _mentorshipRepository.Update(toBeFinishedMentorship);

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse<MentorshipDTO>> GiveFeedbackToFinishedMentorshipAsync(int userId, int mentorshipId, MentorshipFeedbackRequest MentorshipFeedbackRequest)
        {
            var toBeGivenFeedbackMentorship = await _mentorshipRepository.GetWhichIsFinishedByIdAsync(mentorshipId);
            if (toBeGivenFeedbackMentorship == null)
            {
                return new ErrorApiResponse<MentorshipDTO>(ResponseStatus.NotFound, default, ResultMessage.NotFoundFinishedMentorMenteePair);
            }

            bool userRelatedToPair = toBeGivenFeedbackMentorship.Mentee.UserId == userId || toBeGivenFeedbackMentorship.Mentor.UserId == userId;
            if (!userRelatedToPair)
            {
                return new ErrorApiResponse<MentorshipDTO>(ResponseStatus.Forbid, default, ResultMessage.Forbidden);
            }

            bool isUserMentee = userId == toBeGivenFeedbackMentorship.Mentee.UserId;
            
            return isUserMentee
                ? GiveFeedbackFromMentee(toBeGivenFeedbackMentorship, MentorshipFeedbackRequest)
                : GiveFeedbackFromMentor(toBeGivenFeedbackMentorship, MentorshipFeedbackRequest);
        }

        private ApiResponse<MentorshipDTO> GiveFeedbackFromMentee(Mentorship mentorship, MentorshipFeedbackRequest MentorshipFeedbackRequest)
        {
            if (mentorship.MenteeScore != null || mentorship.MenteeComment != null)
            {
                return new ErrorApiResponse<MentorshipDTO>(null, ResultMessage.FeedbackWasAlreadyGiven);
            }

            mentorship.MenteeScore = MentorshipFeedbackRequest.Score;
            mentorship.MenteeComment = MentorshipFeedbackRequest.Comment;

            _mentorshipRepository.Update(mentorship);

            var MentorshipDTO = mapper.Map<MentorshipDTO>(mentorship);
            return new SuccessApiResponse<MentorshipDTO>(MentorshipDTO);
        }

        private ApiResponse<MentorshipDTO> GiveFeedbackFromMentor(Mentorship mentorship, MentorshipFeedbackRequest MentorshipFeedbackRequest)
        {
            if (mentorship.MentorScore != null || mentorship.MentorComment != null)
            {
                return new ErrorApiResponse<MentorshipDTO>(null, ResultMessage.FeedbackWasAlreadyGiven);
            }

            mentorship.MentorScore = MentorshipFeedbackRequest.Score;
            mentorship.MentorComment = MentorshipFeedbackRequest.Comment;

            _mentorshipRepository.Update(mentorship);

            var MentorshipDTO = mapper.Map<MentorshipDTO>(mentorship);
            return new SuccessApiResponse<MentorshipDTO>(MentorshipDTO);
        }
    }
}
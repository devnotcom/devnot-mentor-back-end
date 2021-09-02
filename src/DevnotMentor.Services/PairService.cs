using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevnotMentor.Common;
using DevnotMentor.Common.API;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests;
using DevnotMentor.Common.Requests.Mentorship;
using DevnotMentor.Data.Entities;
using DevnotMentor.Common.Enums;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Services.Interfaces;

namespace DevnotMentor.Services
{
    public class MentorshipService : BaseService, IMentorshipService
    {
        private readonly IMentorshipsRepository pairRepository;
        private readonly IMenteeRepository menteeRepository;
        private readonly IMentorRepository mentorRepository;

        public MentorshipService(IMapper mapper,
                            IMentorshipsRepository pairRepository,
                            IMenteeRepository menteeRepository,
                            IMentorRepository mentorRepository,
                            ILogRepository logger,
                            IDevnotConfigurationContext devnotConfigurationContext
                        ) : base(mapper, logger, devnotConfigurationContext)
        {
            this.pairRepository = pairRepository;
            this.menteeRepository = menteeRepository;
            this.mentorRepository = mentorRepository;
        }

        public async Task<ApiResponse<List<MentorshipDTO>>> GetMentorshipsOfMenteeByUserId(int userId)
        {
            var mentee = await menteeRepository.GetByUserIdAsync(userId);

            if (mentee == null)
            {
                return new ErrorApiResponse<List<MentorshipDTO>>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentee);
            }

            var pairs = mapper.Map<List<MentorshipDTO>>(await pairRepository.GetPairsByUserIdAsync(userId));

            return new SuccessApiResponse<List<MentorshipDTO>>(pairs);
        }

        public async Task<ApiResponse<List<MentorshipDTO>>> GetMentorshipsOfMentorByUserIdAsync(int userId)
        {
            var mentor = await mentorRepository.GetByUserIdAsync(userId);

            if (mentor == null)
            {
                return new ErrorApiResponse<List<MentorshipDTO>>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentor);
            }

            var pairs = mapper.Map<List<MentorshipDTO>>(await pairRepository.GetPairsByUserIdAsync(userId));

            return new SuccessApiResponse<List<MentorshipDTO>>(pairs);
        }

        public async Task<ApiResponse> FinishContinuingPairAsync(int userId, int pairId)
        {
            var pair = await pairRepository.GetWhichIsNotFinishedYetByIdAsync(pairId);

            if (pair == null)
            {
                return new ErrorApiResponse(ResponseStatus.NotFound, ResultMessage.NotFoundNotFinishedMentorMenteePair);
            }

            bool userRelatedToPair = pair.Mentee.UserId == userId || pair.Mentor.UserId == userId;

            if (!userRelatedToPair)
            {
                return new ErrorApiResponse(ResponseStatus.Forbid, ResultMessage.Forbidden);
            }

            pair.State = (int)MentorshipStatus.Finished;
            pair.MentorEndDate = System.DateTime.Now;

            pairRepository.Update(pair);

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse<MentorshipDTO>> GiveFeedbackToFinishedPairAsync(int userId, int pairId, MentorshipFeedbackRequest MentorshipFeedbackRequest)
        {
            var pair = await pairRepository.GetWhichIsFinishedByIdAsync(pairId);

            if (pair == null)
            {
                return new ErrorApiResponse<MentorshipDTO>(ResponseStatus.NotFound, default, ResultMessage.NotFoundFinishedMentorMenteePair);
            }

            bool userRelatedToPair = pair.Mentee.UserId == userId || pair.Mentor.UserId == userId;

            if (!userRelatedToPair)
            {
                return new ErrorApiResponse<MentorshipDTO>(ResponseStatus.Forbid, default, ResultMessage.Forbidden);
            }

            bool isUserMentee = userId == pair.Mentee.UserId;

            return isUserMentee
            ? GiveFeedbackFromMentee(pair, MentorshipFeedbackRequest)
            : GiveFeedbackFromMentor(pair, MentorshipFeedbackRequest);
        }

        private ApiResponse<MentorshipDTO> GiveFeedbackFromMentee(Mentorship pair, MentorshipFeedbackRequest MentorshipFeedbackRequest)
        {
            if (pair.MenteeScore != null || pair.MenteeComment != null)
            {
                return new ErrorApiResponse<MentorshipDTO>(null, ResultMessage.FeedbackWasAlreadyGiven);
            }

            pair.MenteeScore = MentorshipFeedbackRequest.Score;
            pair.MenteeComment = MentorshipFeedbackRequest.Comment;

            pairRepository.Update(pair);

            var MentorshipDTO = mapper.Map<MentorshipDTO>(pair);
            return new SuccessApiResponse<MentorshipDTO>(MentorshipDTO);
        }

        private ApiResponse<MentorshipDTO> GiveFeedbackFromMentor(Mentorship pair, MentorshipFeedbackRequest MentorshipFeedbackRequest)
        {
            if (pair.MentorScore != null || pair.MentorComment != null)
            {
                return new ErrorApiResponse<MentorshipDTO>(null, ResultMessage.FeedbackWasAlreadyGiven);
            }

            pair.MentorScore = MentorshipFeedbackRequest.Score;
            pair.MentorComment = MentorshipFeedbackRequest.Comment;

            pairRepository.Update(pair);

            var MentorshipDTO = mapper.Map<MentorshipDTO>(pair);
            return new SuccessApiResponse<MentorshipDTO>(MentorshipDTO);
        }
    }
}
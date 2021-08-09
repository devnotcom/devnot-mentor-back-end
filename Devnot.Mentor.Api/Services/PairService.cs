using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.PairRequest;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;

namespace DevnotMentor.Api.Services
{
    public class PairService : BaseService, IPairService
    {
        private readonly IMentorMenteePairsRepository pairRepository;
        private readonly IMenteeRepository menteeRepository;
        private readonly IMentorRepository mentorRepository;

        public PairService(IMapper mapper,
                            IMentorMenteePairsRepository pairRepository,
                            IMenteeRepository menteeRepository,
                            IMentorRepository mentorRepository,
                            ILoggerRepository logger,
                            IDevnotConfigurationContext devnotConfigurationContext
                        ) : base(mapper, logger, devnotConfigurationContext)
        {
            this.pairRepository = pairRepository;
            this.menteeRepository = menteeRepository;
            this.mentorRepository = mentorRepository;
        }

        public async Task<ApiResponse<List<PairDto>>> GetMentorshipsOfMenteeByUserId(int userId)
        {
            var mentee = await menteeRepository.GetByUserIdAsync(userId);

            if (mentee == null)
            {
                return new ErrorApiResponse<List<PairDto>>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentee);
            }

            var pairs = mapper.Map<List<PairDto>>(await pairRepository.GetPairsByUserIdAsync(userId));

            return new SuccessApiResponse<List<PairDto>>(pairs);
        }

        public async Task<ApiResponse<List<PairDto>>> GetMentorshipsOfMentorByUserIdAsync(int userId)
        {
            var mentor = await mentorRepository.GetByUserIdAsync(userId);

            if (mentor == null)
            {
                return new ErrorApiResponse<List<PairDto>>(ResponseStatus.NotFound, data: default, message: ResultMessage.NotFoundMentor);
            }

            var pairs = mapper.Map<List<PairDto>>(await pairRepository.GetPairsByUserIdAsync(userId));

            return new SuccessApiResponse<List<PairDto>>(pairs);
        }

        public async Task<ApiResponse> FinishContinuingPairAsync(int userId, int pairId)
        {
            var pair = await pairRepository.GetWhichIsNotFinishedYetByIdAsync(pairId);

            if (pair == null)
            {
                return new ErrorApiResponse(ResponseStatus.NotFound, ResultMessage.NotFoundNotFinishedMentorMenteePair);
            }

            bool checkUserHasThePair = pair.Mentee.UserId == userId || pair.Mentor.UserId == userId;

            if (!checkUserHasThePair)
            {
                return new ErrorApiResponse(ResponseStatus.Forbid, ResultMessage.Forbidden);
            }

            pair.State = MentorMenteePairStatus.Finished.ToInt();
            pair.MentorEndDate = System.DateTime.Now;

            pairRepository.Update(pair);

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse<PairDto>> GiveFeedbackToFinishedPairAsync(int userId, int pairId, PairFeedbackRequest pairFeedbackRequest)
        {
            var pair = await pairRepository.GetWhichIsFinishedByIdAsync(pairId);

            if (pair == null)
            {
                return new ErrorApiResponse<PairDto>(ResponseStatus.NotFound, default, ResultMessage.NotFoundFinishedMentorMenteePair);
            }

            bool checkUserHasThePair = pair.Mentee.UserId == userId || pair.Mentor.UserId == userId;

            if (!checkUserHasThePair)
            {
                return new ErrorApiResponse<PairDto>(ResponseStatus.Forbid, default, ResultMessage.Forbidden);
            }

            bool checkUserIsMentee = userId == pair.Mentee.UserId;

            return checkUserIsMentee
            ? GiveFeedbackFromMentee(pair, pairFeedbackRequest)
            : GiveFeedbackFromMentor(pair, pairFeedbackRequest);
        }

        private ApiResponse<PairDto> GiveFeedbackFromMentee(MentorMenteePairs pair, PairFeedbackRequest pairFeedbackRequest)
        {
            if (pair.MenteeScore != null || pair.MenteeComment != null)
            {
                return new ErrorApiResponse<PairDto>(null, ResultMessage.FeedbackWasAlreadyGiven);
            }

            pair.MenteeScore = pairFeedbackRequest.Score;
            pair.MenteeComment = pairFeedbackRequest.Comment;

            pairRepository.Update(pair);

            var pairDto = mapper.Map<PairDto>(pair);
            return new SuccessApiResponse<PairDto>(pairDto);
        }

        private ApiResponse<PairDto> GiveFeedbackFromMentor(MentorMenteePairs pair, PairFeedbackRequest pairFeedbackRequest)
        {
            if (pair.MentorScore != null || pair.MentorComment != null)
            {
                return new ErrorApiResponse<PairDto>(null, ResultMessage.FeedbackWasAlreadyGiven);
            }

            pair.MentorScore = pairFeedbackRequest.Score;
            pair.MentorComment = pairFeedbackRequest.Comment;

            pairRepository.Update(pair);

            var pairDto = mapper.Map<PairDto>(pair);
            return new SuccessApiResponse<PairDto>(pairDto);
        }
    }
}
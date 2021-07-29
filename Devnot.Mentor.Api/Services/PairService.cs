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

        public PairService(IMapper mapper,
                            ILoggerRepository logger,
                            IDevnotConfigurationContext devnotConfigurationContext, IMentorMenteePairsRepository pairRepository) : base(mapper, logger, devnotConfigurationContext)
        {
            this.pairRepository = pairRepository;
        }

        public async Task<ApiResponse> FinishByIdAndAuthorizedUser(int authorizedUserId, int pairId)
        {
            var pair = await pairRepository.GetByIdAndStatusNotFinishedAsync(pairId);
            if (pair == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundNotFinishedMentorMenteePair);
            }

            bool authorizedUserRelatedToPair = pair.Mentee.UserId == authorizedUserId || pair.Mentor.UserId == authorizedUserId;
            if (!authorizedUserRelatedToPair)
            {
                return new ErrorApiResponse(ResultMessage.Forbidden);
            }

            pair.State = MentorMenteePairStatus.Finished.ToInt();
            pair.MentorEndDate = System.DateTime.Now;
            pairRepository.Update(pair);
            return new SuccessApiResponse();
        }

        public async Task<ApiResponse<PairDto>> FeedbackByIdAndAuthorizedUser(int authorizedUserId, int pairId, PairFeedbackRequest pairFeedbackRequest)
        {
            var pair = await pairRepository.GetByIdAndStatusFinishedAsync(pairId);
            if (pair == null)
            {
                return new ErrorApiResponse<PairDto>(null, ResultMessage.NotFoundFinishedMentorMenteePair);
            }

            bool authorizedUserRelatedToPair = pair.Mentee.UserId == authorizedUserId || pair.Mentor.UserId == authorizedUserId;
            if (!authorizedUserRelatedToPair)
            {
                return new ErrorApiResponse<PairDto>(null, ResultMessage.Forbidden);
            }

            bool authorizedUserMenteeForPair = authorizedUserId == pair.Mentee.UserId;
            return authorizedUserMenteeForPair
            ? FeedbackFromMentee(pair, pairFeedbackRequest)
            : FeedbackFromMentor(pair, pairFeedbackRequest);
        }

        private ApiResponse<PairDto> FeedbackFromMentee(MentorMenteePairs pair, PairFeedbackRequest pairFeedbackRequest)
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

        private ApiResponse<PairDto> FeedbackFromMentor(MentorMenteePairs pair, PairFeedbackRequest pairFeedbackRequest)
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
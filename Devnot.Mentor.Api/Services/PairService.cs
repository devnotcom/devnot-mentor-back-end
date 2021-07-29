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
        private readonly IMentorMenteePairsRepository pairsRepository;

        public PairService(IMapper mapper,
                            ILoggerRepository logger,
                            IDevnotConfigurationContext devnotConfigurationContext, IMentorMenteePairsRepository pairsRepository) : base(mapper, logger, devnotConfigurationContext)
        {
            this.pairsRepository = pairsRepository;
        }

        public async Task<ApiResponse> FinishByIdAndAuthorizedUser(int authorizedUserId, int pairId)
        {
            var pair = await pairsRepository.GetByIdAndStatusNotFinishedAsync(pairId);
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
            pairsRepository.Update(pair);
            return new SuccessApiResponse();
        }

        public async Task<ApiResponse<PairsDto>> FeedbackByIdAndAuthorizedUser(int authorizedUserId, int pairId, PairFeedbackRequest pairFeedbackRequest)
        {
            var pair = await pairsRepository.GetByIdAndStatusFinishedAsync(pairId);
            if (pair == null)
            {
                return new ErrorApiResponse<PairsDto>(null, ResultMessage.NotFoundFinishedMentorMenteePair);
            }

            bool authorizedUserRelatedToPair = pair.Mentee.UserId == authorizedUserId || pair.Mentor.UserId == authorizedUserId;
            if (!authorizedUserRelatedToPair)
            {
                return new ErrorApiResponse<PairsDto>(null, ResultMessage.Forbidden);
            }

            bool authorizedUserMenteeForPair = authorizedUserId == pair.Mentee.UserId;
            return authorizedUserMenteeForPair
            ? FeedbackFromMentee(pair, pairFeedbackRequest)
            : FeedbackFromMentor(pair, pairFeedbackRequest);
        }

        private ApiResponse<PairsDto> FeedbackFromMentee(MentorMenteePairs pair, PairFeedbackRequest pairFeedbackRequest)
        {
            if (pair.MenteeScore != null || pair.MenteeComment != null)
            {
                return new ErrorApiResponse<PairsDto>(null, ResultMessage.FeedbackWasAlreadyGiven);
            }

            pair.MenteeScore = pairFeedbackRequest.Score;
            pair.MenteeComment = pairFeedbackRequest.Comment;
            pairsRepository.Update(pair);
            var pairDto = mapper.Map<PairsDto>(pair);

            return new SuccessApiResponse<PairsDto>(pairDto);
        }

        private ApiResponse<PairsDto> FeedbackFromMentor(MentorMenteePairs pair, PairFeedbackRequest pairFeedbackRequest)
        {
            if (pair.MentorScore != null || pair.MentorComment != null)
            {
                return new ErrorApiResponse<PairsDto>(null, ResultMessage.FeedbackWasAlreadyGiven);
            }

            pair.MentorScore = pairFeedbackRequest.Score;
            pair.MentorComment = pairFeedbackRequest.Comment;
            pairsRepository.Update(pair);
            var pairDto = mapper.Map<PairsDto>(pair);

            return new SuccessApiResponse<PairsDto>(pairDto);
        }
    }
}
using System.Threading.Tasks;
using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
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

        public async Task<ApiResponse> FinisForAuthorizedUserById(int authorizedUserId, int pairId)
        {
            var pair = await pairsRepository.GetForStatusNotFinishedByIdAndAsync(pairId);
            if (pair == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentorMenteePair);
            }

            bool authorizedUserRelatedToPair = pair.Mentee.UserId == authorizedUserId || pair.Mentor.UserId == authorizedUserId;
            if (!authorizedUserRelatedToPair)
            {
                return new ErrorApiResponse(ResultMessage.Forbidden);
            }

            pair.State = MentorMenteePairStatus.Finished.ToInt();
            pairsRepository.Update(pair);
            return new SuccessApiResponse();
        }

        public async Task<ApiResponse> FeedbackForAuthorizedUserById(int authorizedUserId, int pairId, PairFeedbackRequest pairFeedbackRequest)
        {
            var pair = await pairsRepository.GetForStatusFinishedByIdAndAsync(pairId);
            if (pair == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentorMenteePair);
            }

            bool authorizedUserRelatedToPair = pair.Mentee.UserId == authorizedUserId || pair.Mentor.UserId == authorizedUserId;
            if (!authorizedUserRelatedToPair)
            {
                return new ErrorApiResponse(ResultMessage.Forbidden);
            }

            bool authorizedUserMenteeForPair = authorizedUserId == pair.Mentee.UserId;
            return authorizedUserMenteeForPair
            ? FeedbackFromMentee(pair, pairFeedbackRequest)
            : FeedbackFromMentor(pair, pairFeedbackRequest);
        }

        private ApiResponse FeedbackFromMentee(MentorMenteePairs pair, PairFeedbackRequest pairFeedbackRequest)
        {
            if (pair.MenteeScore != null || pair.MenteeComment != null)
            {
                return new ErrorApiResponse(ResultMessage.FeedbackAlreadyGiven);
            }

            pair.MenteeScore = pairFeedbackRequest.Score;
            pair.MentorComment = pairFeedbackRequest.Comment;
            pairsRepository.Update(pair);
            return new SuccessApiResponse();
        }
        
        private ApiResponse FeedbackFromMentor(MentorMenteePairs pair, PairFeedbackRequest pairFeedbackRequest)
        {
            if (pair.MentorScore != null || pair.MentorComment != null)
            {
                return new ErrorApiResponse(ResultMessage.FeedbackAlreadyGiven);
            }

            pair.MentorScore = pairFeedbackRequest.Score;
            pair.MentorComment = pairFeedbackRequest.Comment;
            pairsRepository.Update(pair);
            return new SuccessApiResponse();
        }
    }
}
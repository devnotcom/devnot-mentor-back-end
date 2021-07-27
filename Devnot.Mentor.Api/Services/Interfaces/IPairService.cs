using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.PairRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IPairService
    {
        /// <summary>
        /// Finish a not finished pair, If Authorized User is a mentee or mentor for the pair.
        /// </summary>
        /// <param name="authorizedUserId"></param>
        /// <param name="pairId"></param>
        /// <returns><see cref="ApiResponse"/></returns>
        Task<ApiResponse> FinishByIdAndAuthorizedUser(int authorizedUserId, int pairId);

        /// <summary>
        /// Add feedback as a mentor or mentee to finished pair, If Authorized User is a mentee or mentor for the pair.
        /// <para>The decision mechanism for who makes the feedback is related to the Authorized User.</para> 
        /// </summary>
        /// <param name="authorizedUserId"></param>
        /// <param name="pairId"></param>
        /// <returns><see cref="PairsDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<PairsDto>> FeedbackByIdAndAuthorizedUser(int authorizedUserId, int pairId, PairFeedbackRequest pairFeedbackRequest);
    }
}
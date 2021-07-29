using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.PairRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IPairService
    {
        /// <summary>
        /// Returns mentorship processes.
        /// </summary>
        /// <param name="userId">Mentee UserId</param>
        /// <returns>List of <see cref="PairDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<PairDto>>> GetMentorshipsOfMenteeByUserId(int userId);

        /// <summary>
        /// Returns mentorship processes.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="PairDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<PairDto>>> GetMentorshipsOfMentorByUserIdAsync(int userId);

        /// <summary>
        /// Finish a not finished pair, If Authorized User is a mentee or mentor for the pair.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pairId"></param>
        /// <returns><see cref="ApiResponse"/></returns>
        Task<ApiResponse> FinishContinuingPairAsync(int userId, int pairId);

        /// <summary>
        /// Add feedback as a mentor or mentee to finished pair, If Authorized User is a mentee or mentor for the pair.
        /// <para>The decision mechanism for who makes the feedback is related to the Authorized User.</para> 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pairId"></param>
        /// <returns><see cref="PairDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<PairDto>> GiveFeedbackToFinishedPairAsync(int userId, int pairId, PairFeedbackRequest pairFeedbackRequest);
    }
}
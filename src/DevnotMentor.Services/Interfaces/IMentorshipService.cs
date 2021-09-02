using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentorship;

namespace DevnotMentor.Services.Interfaces
{
    public interface IMentorshipService
    {
        /// <summary>
        /// Returns mentorship processes.
        /// </summary>
        /// <param name="userId">Mentee UserId</param>
        /// <returns>List of <see cref="MentorshipDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorshipDTO>>> GetMentorshipsOfMenteeByUserId(int userId);

        /// <summary>
        /// Returns mentorship processes.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MentorshipDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorshipDTO>>> GetMentorshipsOfMentorByUserIdAsync(int userId);

        /// <summary>
        /// Finish a not finished pair, If Authenticated User is a mentee or mentor for the pair.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pairId"></param>
        /// <returns><see cref="ApiResponse"/></returns>
        Task<ApiResponse> FinishContinuingMentorshipAsync(int userId, int pairId);

        /// <summary>
        /// Add feedback as a mentor or mentee to finished pair, If Authenticated User is a mentee or mentor for the pair.
        /// <para>The decision mechanism for who makes the feedback is related to the Authenticated User.</para> 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pairId"></param>
        /// <returns><see cref="MentorshipDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<MentorshipDTO>> GiveFeedbackToFinishedMentorshipAsync(int userId, int pairId, MentorshipFeedbackRequest MentorshipFeedbackRequest);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentorship;

namespace DevnotMentor.Business.Services.Interfaces
{
    public interface IMentorshipService
    {
        /// <summary>
        /// Returns mentorship processes for mentee.
        /// </summary>
        /// <returns>List of <see cref="MentorshipDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorshipDTO>>> GetMentorshipsByMenteeUserIdAsync(int menteeUserId);

        /// <summary>
        /// Returns mentorship processes for mentor.
        /// </summary>
        /// <returns>List of <see cref="MentorshipDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorshipDTO>>> GetMentorshipsByMentorUserIdAsync(int mentorUserId);

        /// <summary>
        /// Finishs a not finished mentorship, If Authenticated User is a mentee or mentor for the mentorship.
        /// </summary>
        /// <returns><see cref="ApiResponse"/></returns>
        Task<ApiResponse> FinishContinuingMentorshipAsync(int userId, int mentorshipId);

        /// <summary>
        /// Add feedback as a mentor or mentee to finished mentorship, If Authenticated User is a mentee or mentor for the mentorship.
        /// <para>The decision mechanism for who makes the feedback is related to the Authenticated User.</para> 
        /// </summary>
        /// <returns><see cref="MentorshipDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<MentorshipDTO>> GiveFeedbackToFinishedMentorshipAsync(int userId, int mentorshipId, MentorshipFeedbackRequest MentorshipFeedbackRequest);
    }
}
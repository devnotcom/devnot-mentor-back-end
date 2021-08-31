using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IApplicationService
    {
        /// <summary>
        /// Returns applications with mentee and mentor informations
        /// </summary>
        /// <param name="authenticatedUserId">Authenticated Mentor OR Mentee User Id</param>
        /// <returns>List of <see cref="MentorApplicationsDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorApplicationsDto>>> GetApplicationsByUserIdAsync(int authenticatedUserId);

        /// <summary>
        /// Approve the waiting application
        /// <para>If Mentee or Mentor have already reached the max count for the continuing mentorship, cannot be approved.</para>
        /// </summary>
        /// <param name="authenticatedUserId">Authenticated Mentor User Id</param>
        /// <param name="toBeApprovedApplicationId">Id of the application to be approved</param>
        /// <returns><see cref="ApiResponse"/></returns>
        Task<ApiResponse> ApproveWaitingApplicationByIdAsync(int authenticatedUserId, int toBeApprovedApplicationId);

        /// <summary>
        /// Reject the waiting application
        /// </summary>
        /// <param name="authenticatedUserId">Authenticated Mentor User Id</param>
        /// <param name="toBeRejectedApplicationId">Id of the application to be rejected</param>
        /// <returns><see cref="ApiResponse"/></returns>
        Task<ApiResponse> RejectWaitingApplicationByIdAsync(int authenticatedUserId, int toBeRejectedApplicationId);

        /// <summary>
        /// Create application via <see cref="ApplicationRequest"/>
        /// <para>If there is any waiting application between Mentee and Mentor, cannot be created.</para>
        /// </summary>
        /// <param name="request"><see cref="ApplicationRequest"/></param>
        /// <returns><see cref="ApiResponse"/></returns>
        Task<ApiResponse> CreateApplicationAsync(ApplicationRequest request);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.Mentee;

namespace DevnotMentor.Services.Interfaces
{
    public interface IApplicationService
    {
        /// <summary>
        /// Returns applications with mentee and mentor informations.
        /// </summary>
        /// <param name="authenticatedUserId">Authenticated Mentor OR Mentee User Id</param>
        /// <returns>List of <see cref="ApplicationDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<ApplicationDTO>>> GetApplicationsByUserIdAsync(int authenticatedUserId);

        /// <summary>
        /// Approves the waiting application
        /// <para>If Mentee or Mentor have already reached the max count for the continuing mentorship, cannot be approved.</para>
        /// </summary>
        /// <param name="authenticatedUserId">Authenticated Mentor User Id</param>
        /// <param name="toBeApprovedApplicationId">Id of the application to be approved</param>
        /// <returns><see cref="ApiResponse"/></returns>
        Task<ApiResponse> ApproveWaitingApplicationByIdAsync(int authenticatedUserId, int toBeApprovedApplicationId);

        /// <summary>
        /// Rejects the waiting application.
        /// </summary>
        /// <param name="authenticatedUserId">Authenticated Mentor User Id</param>
        /// <param name="toBeRejectedApplicationId">Id of the application to be rejected</param>
        /// <returns><see cref="ApiResponse"/></returns>
        Task<ApiResponse> RejectWaitingApplicationByIdAsync(int authenticatedUserId, int toBeRejectedApplicationId);

        /// <summary>
        /// Creates application.
        /// <para>If there is any waiting application between Mentee and Mentor, cannot be created.</para>
        /// </summary>
        /// <returns>Created <see cref="ApplicationDTO" /> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<ApplicationDTO>> CreateApplicationAsync(ApplicationRequest request);
    }
}
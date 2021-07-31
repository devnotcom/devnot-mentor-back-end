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
        /// Accept the application
        /// <para>If Mentee or Mentor have already reached the max count for the continuing mentorship, Mentor can't accept the application.</para>
        /// </summary>
        /// <param name="authenticatedUserId">Authenticated Mentor User Id</param>
        /// <param name="applicationId">Id of the application to be accepted</param>
        /// <returns></returns>
        Task<ApiResponse> AcceptApplicationByIdAsync(int authenticatedUserId, int applicationId);

        /// <summary>
        /// Reject the application
        /// </summary>
        /// <param name="authenticatedUserId">Authenticated Mentor User Id</param>
        /// <param name="applicationId">Id of the application to be rejected</param>
        /// <returns></returns>
        Task<ApiResponse> RejectApplicationByIdAsync(int authenticatedUserId, int applicationId);

        /// <summary>
        /// Create application via <see cref="ApplicationRequest"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse> CreateApplicationAsync(ApplicationRequest request);
    }
}
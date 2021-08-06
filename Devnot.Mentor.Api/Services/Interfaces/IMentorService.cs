using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.CommonRequest;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMentorService
    {
        /// <summary>
        /// Get mentor profile by specified user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ApiResponse<MentorDto>> GetMentorProfileAsync(string userName);

        /// <summary>
        /// Create mentor profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<MentorDto>> CreateMentorProfileAsync(CreateMentorProfileRequest request);

        /// <summary>
        /// Accept mentee application
        /// </summary>
        /// <param name="mentorUserId">User id who is mentor</param>
        /// <param name="mentorId">Mentor id</param>
        /// <param name="menteeId">Mentee id</param>
        /// <returns></returns>
        Task<ApiResponse> AcceptMenteeAsync(int mentorUserId, int mentorId, int menteeId);

        /// <summary>
        /// Reject mentee application.
        /// </summary>
        /// <param name="mentorUserId">User id who is mentor</param>
        /// <param name="mentorId">Mentor id</param>
        /// <param name="menteeId">Mentee id</param>
        /// <returns></returns>
        Task<ApiResponse> RejectMenteeAsync(int mentorUserId, int mentorId, int menteeId);

        /// <summary>
        /// Returns mentees who are paired with mentor.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MenteeDto"/>  inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MenteeDto>>> GetPairedMenteesByUserIdAsync(int userId);

        /// <summary>
        /// Returns mentoring applications with mentee informations.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MentorApplicationsDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorApplicationsDto>>> GetApplicationsByUserIdAsync(int userId);

        /// <summary>
        /// Get mentor list which contains properties in search request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<List<MentorDto>>> SearchAsync(SearchRequest request);
    }
}

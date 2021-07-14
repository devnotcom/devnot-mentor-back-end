using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMentorService
    {
        /// <summary>
        /// Returns mentorship processes.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="PairsDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<PairsDto>>> GetMentorshipsByUserId(int userId);

        Task<ApiResponse<MentorDto>> GetMentorProfile(string userName);

        Task<ApiResponse<MentorDto>> CreateMentorProfile(CreateMentorProfileRequest request);

        Task<ApiResponse> AcceptMentee(int mentorUserId, int mentorId, int menteeId);

        Task<ApiResponse> RejectMentee(int mentorUserId, int mentorId, int menteeId);

        /// <summary>
        /// Returns mentees who are paired with mentor.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MenteeDto"/>  inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MenteeDto>>> GetPairedMenteesByUserId(int userId);

        /// <summary>
        /// Returns mentoring applications with mentee informations.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MentorApplicationsDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorApplicationsDto>>> GetApplicationsByUserId(int userId);
    }
}

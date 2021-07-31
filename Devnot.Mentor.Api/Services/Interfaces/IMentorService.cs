using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMentorService
    {
        Task<ApiResponse<MentorDto>> GetMentorProfileAsync(string userName);

        Task<ApiResponse<MentorDto>> CreateMentorProfileAsync(CreateMentorProfileRequest request);

        Task<ApiResponse> AcceptMenteeAsync(int mentorUserId, int mentorId, int menteeId);

        Task<ApiResponse> RejectMenteeAsync(int mentorUserId, int mentorId, int menteeId);

        /// <summary>
        /// Returns mentees who are paired with mentor.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MenteeDto"/>  inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MenteeDto>>> GetPairedMenteesByUserIdAsync(int userId);
    }
}

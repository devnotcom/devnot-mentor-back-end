using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMentorService
    {
        Task<ApiResponse<MentorDto>> GetMentorProfile(string userName);

        Task<ApiResponse<MentorDto>> CreateMentorProfile(CreateMentorProfileRequest request);

        Task<ApiResponse> AcceptMentee(int mentorUserId, int mentorId, int menteeId);

        Task<ApiResponse> RejectMentee(int mentorUserId, int mentorId, int menteeId);

        /// <summary>
        /// Returns mentees who are paired with mentor.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MenteeDto"/>  inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse> GetPairedMenteesByUserId(int userId);

        /// <summary>
        /// Returns mentoring applications with mentee informations.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MentorApplicationsDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse> GetApplicationsByUserId(int userId);
    }
}

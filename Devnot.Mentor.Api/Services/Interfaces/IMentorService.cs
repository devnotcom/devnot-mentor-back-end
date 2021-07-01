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
        /// This method returns mentees who are paired with mentor.
        /// </summary>
        /// <param name="userName">Mentor UserName</param>
        /// <returns>List of MenteeDTO inside the ApiResponse</returns>
        Task<ApiResponse> GetMentees(string userName);
    }
}

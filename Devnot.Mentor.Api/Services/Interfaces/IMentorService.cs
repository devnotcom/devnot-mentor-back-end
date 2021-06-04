using DevnotMentor.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMentorService
    {
        Task<ApiResponse<MentorProfileModel>> GetMentorProfile(string userName);
        Task<ApiResponse<MentorProfileModel>> CreateMentorProfile(MentorProfileModel model);
        Task<ApiResponse> AcceptMentee(int mentorUserId, int menteeUserId);
        Task<ApiResponse> RejectMentee(int mentorUserId, int menteeUserId);
    }
}

using DevnotMentor.Api.Models;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMenteeService
    {
        Task<ApiResponse<MenteeProfileModel>> GetMenteeProfile(string userName);

        Task<ApiResponse<MenteeProfileModel>> CreateMenteeProfile(MenteeProfileModel model);

        //void UpdateMenteeProfile(MenteeProfileModel model);
        Task<ApiResponse> ApplyToMentor(ApplyMentorModel model);
    }
}

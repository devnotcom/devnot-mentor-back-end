using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMenteeService
    {
        Task<ApiResponse<MenteeDto>> GetMenteeProfile(string userName);

        Task<ApiResponse<MenteeDto>> CreateMenteeProfile(CreateMenteeProfileRequest request);

        Task<ApiResponse> ApplyToMentor(ApplyToMentorRequest request);
    }
}

using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMenteeService
    {
        Task<ApiResponse<MenteeDto>> GetMenteeProfile(string userName);

        /// <summary>
        /// This method returns mentors who are paired with mentee.
        /// </summary>
        /// <param name="userName">Mentee UserName</param>
        /// <returns>List of MentorDTO inside the ApiResponse</returns>
        Task<ApiResponse> GetMentors(string userName);

        /// <summary>
        /// This method returns non approved mentoring applications with mentor informations.
        /// </summary>
        /// <param name="userName">Mentee UserName</param>
        /// <returns>List of MentorAplicationsDTO inside the ApiResponse</returns>
        Task<ApiResponse> GetApplicationsNotIncludeApproveds(string userName);

        Task<ApiResponse<MenteeDto>> CreateMenteeProfile(CreateMenteeProfileRequest request);

        Task<ApiResponse> ApplyToMentor(ApplyToMentorRequest request);
    }
}

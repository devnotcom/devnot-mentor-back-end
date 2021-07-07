using System.Collections.Generic;
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
        /// Returns mentors who are paired with mentee.
        /// </summary>
        /// <param name="userId">Mentee UserId</param>
        /// <returns>List of <see cref="MentorDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorDto>>> GetPairedMentorsByUserId(int userId);

        /// <summary>
        /// Returns mentoring applications with mentor informations.
        /// </summary>
        /// <param name="userId">Mentee UserId</param>
        /// <returns>List of <see cref="MentorApplicationsDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorApplicationsDto>>> GetApplicationsByUserId(int userId);

        Task<ApiResponse<MenteeDto>> CreateMenteeProfile(CreateMenteeProfileRequest request);

        Task<ApiResponse> ApplyToMentor(ApplyToMentorRequest request);
    }
}

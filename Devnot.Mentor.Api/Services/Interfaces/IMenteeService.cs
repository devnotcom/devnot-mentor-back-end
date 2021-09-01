using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.CommonRequest;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMenteeService
    {
        /// <summary>
        /// Get mentee profile by user name.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ApiResponse<MenteeDto>> GetMenteeProfileAsync(string userName);

        /// <summary>
        /// Returns mentors who are paired with mentee.
        /// </summary>
        /// <param name="userId">Mentee UserId</param>
        /// <returns>List of <see cref="MentorDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorDto>>> GetPairedMentorsByUserIdAsync(int userId);
        
        /// <summary>
        /// Create mentee profile.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<MenteeDto>> CreateMenteeProfileAsync(CreateMenteeProfileRequest request);

        /// <summary>
        /// Get mentee list which contains properties in search request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<List<MenteeDto>>> SearchAsync(SearchRequest request);
    }
}

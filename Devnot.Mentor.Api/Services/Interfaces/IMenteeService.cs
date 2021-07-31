using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMenteeService
    {
        Task<ApiResponse<MenteeDto>> GetMenteeProfileAsync(string userName);

        /// <summary>
        /// Returns mentors who are paired with mentee.
        /// </summary>
        /// <param name="userId">Mentee UserId</param>
        /// <returns>List of <see cref="MentorDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorDto>>> GetPairedMentorsByUserIdAsync(int userId);

        Task<ApiResponse<MenteeDto>> CreateMenteeProfileAsync(CreateMenteeProfileRequest request);
    }
}

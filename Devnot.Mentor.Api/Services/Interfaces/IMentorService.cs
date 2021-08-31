using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.CommonRequest;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMentorService
    {
        /// <summary>
        /// Get mentor profile by specified user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ApiResponse<MentorDto>> GetMentorProfileAsync(string userName);

        /// <summary>
        /// Create mentor profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<MentorDto>> CreateMentorProfileAsync(CreateMentorProfileRequest request);

        /// <summary>
        /// Returns mentees who are paired with mentor.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MenteeDto"/>  inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MenteeDto>>> GetPairedMenteesByUserIdAsync(int userId);
        
        /// <summary>
        /// Get mentor list which contains properties in search request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<List<MentorDto>>> SearchAsync(SearchRequest request);
    }
}

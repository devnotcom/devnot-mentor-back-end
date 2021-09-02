using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests;
using DevnotMentor.Common.Requests.Mentor;

namespace DevnotMentor.Services.Interfaces
{
    public interface IMentorService
    {
        /// <summary>
        /// Get mentor profile by specified user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ApiResponse<MentorDTO>> GetMentorProfileAsync(string userName);

        /// <summary>
        /// Create mentor profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<MentorDTO>> CreateMentorProfileAsync(CreateMentorProfileRequest request);

        /// <summary>
        /// Returns mentees who are paired with mentor.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MenteeDTO"/>  inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MenteeDTO>>> GetPairedMenteesByUserIdAsync(int userId);
        
        /// <summary>
        /// Get mentor list which contains properties in search request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<List<MentorDTO>>> SearchAsync(SearchRequest request);
    }
}

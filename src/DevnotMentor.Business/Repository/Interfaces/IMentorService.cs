using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests;
using DevnotMentor.Common.Requests.Mentor;

namespace DevnotMentor.Business.Repository.Interfaces
{
    public interface IMentorService
    {
        /// <summary>
        /// Gets mentor profile by user name.
        /// </summary>
        /// <returns><see cref="MentorDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<MentorDTO>> GetMentorProfileByUserNameAsync(string userName);

        /// <summary>
        /// Creates mentor profile.
        /// </summary>
        /// <returns>Created <see cref="MentorDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<MentorDTO>> CreateMentorProfileAsync(CreateMentorProfileRequest request);

        /// <summary>
        /// Returns mentees who are paired with mentor.
        /// </summary>
        /// <param name="userId">Mentor UserId</param>
        /// <returns>List of <see cref="MenteeDTO"/>  inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MenteeDTO>>> GetPairedMenteesByUserIdAsync(int userId);
        
        /// <summary>
        /// Gets mentors by <see cref="SearchRequest" />
        /// </summary>
        /// <returns>List of <see cref="MentorDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorDTO>>> SearchAsync(SearchRequest request);
    }
}

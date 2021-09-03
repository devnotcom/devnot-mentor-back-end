using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests;
using DevnotMentor.Common.Requests.Mentee;

namespace DevnotMentor.Business.Services.Interfaces
{
    public interface IMenteeService
    {
        /// <summary>
        /// Gets mentee profile by user name.
        /// </summary>
        /// <returns><see cref="MenteeDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<MenteeDTO>> GetMenteeProfileByUserNameAsync(string userName);

        /// <summary>
        /// Returns mentors who are paired with mentee.
        /// </summary>
        /// <param name="userId">Mentee UserId</param>
        /// <returns>List of <see cref="MentorDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorDTO>>> GetPairedMentorsByUserIdAsync(int userId);

        /// <summary>
        /// Creates mentee profile.
        /// </summary>
        /// <returns>Created <see cref="MenteeDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<MenteeDTO>> CreateMenteeProfileAsync(CreateMenteeProfileRequest request);

        /// <summary>
        /// Gets mentees by <see cref="SearchRequest" />
        /// </summary>
        /// <returns>List of <see cref="MenteeDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MenteeDTO>>> SearchAsync(SearchRequest request);
    }
}

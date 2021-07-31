using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Dto;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IApplicationService
    {
        /// <summary>
        /// Returns applications with mentee and mentor informations
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of <see cref="MentorApplicationsDto"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorApplicationsDto>>> GetApplicationsByUserIdAsync(int userId);
    }
}
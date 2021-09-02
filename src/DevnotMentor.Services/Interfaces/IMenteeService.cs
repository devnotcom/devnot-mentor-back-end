﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests;
using DevnotMentor.Common.Requests.Mentee;

namespace DevnotMentor.Services.Interfaces
{
    public interface IMenteeService
    {
        /// <summary>
        /// Get mentee profile by user name.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ApiResponse<MenteeDTO>> GetMenteeProfileAsync(string userName);

        /// <summary>
        /// Returns mentors who are paired with mentee.
        /// </summary>
        /// <param name="userId">Mentee UserId</param>
        /// <returns>List of <see cref="MentorDTO"/> inside the <see cref="ApiResponse"/></returns>
        Task<ApiResponse<List<MentorDTO>>> GetPairedMentorsByUserIdAsync(int userId);
        
        /// <summary>
        /// Create mentee profile.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<MenteeDTO>> CreateMenteeProfileAsync(CreateMenteeProfileRequest request);

        /// <summary>
        /// Get mentee list which contains properties in search request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<List<MenteeDTO>>> SearchAsync(SearchRequest request);
    }
}
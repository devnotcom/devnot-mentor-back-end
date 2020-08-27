using DevnotMentor.Api.Common;
using DevnotMentor.Api.Controllers;
using DevnotMentor.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IMenteeService
    {
        Task<ApiResponse<MenteeProfileModel>> GetMenteeProfile(string userName);

        Task<ApiResponse<MenteeProfileModel>> CreateMenteeProfile(MenteeProfileModel model);

        //void UpdateMenteeProfile(MenteeProfileModel model);
        Task<ApiResponse> ApplyToMentor(ApplyMentorModel model);
    }
}

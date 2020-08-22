using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<User>> Login(LoginModel model);
        Task<ApiResponse<User>> Register(UserModel model);
        Task<ApiResponse<bool>> ChangePassword(PasswordUpdateModel model);
        Task<ApiResponse<User>> Update(UserUpdateModel model);
        Task<ApiResponse> RemindPassword(string email);
        Task<ApiResponse> RemindPasswordComplete(RemindPasswordCompleteModel model);
    }
}

using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Models;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;

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

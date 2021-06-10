using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Models;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Request.UserRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<User>> Login(LoginModel model);
        Task<ApiResponse<User>> Register(RegisterUserRequest request);
        Task<ApiResponse> ChangePassword(UpdatePasswordRequest request);
        Task<ApiResponse<User>> Update(UpdateUserRequest request);
        Task<ApiResponse> RemindPassword(string email);
        Task<ApiResponse> RemindPasswordComplete(CompleteRemindPasswordRequest request);
    }
}

using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Request.UserRequest;
using DevnotMentor.Api.CustomEntities.Response.UserResponse;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserLoginResponse>> Login(UserLoginRequest request);
        Task<ApiResponse> Register(RegisterUserRequest request);
        Task<ApiResponse> ChangePassword(UpdatePasswordRequest request);
        Task<ApiResponse> Update(UpdateUserRequest request);
        Task<ApiResponse> RemindPassword(string email);
        Task<ApiResponse> RemindPasswordComplete(CompleteRemindPasswordRequest request);
    }
}

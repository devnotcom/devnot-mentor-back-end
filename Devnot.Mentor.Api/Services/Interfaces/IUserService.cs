using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Request.UserRequest;
using DevnotMentor.Api.CustomEntities.Response.UserResponse;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserLoginResponse>> LoginAsync(UserLoginRequest request);
        Task<ApiResponse> RegisterAsync(RegisterUserRequest request);
        Task<ApiResponse> ChangePasswordAsync(UpdatePasswordRequest request);
        Task<ApiResponse> UpdateAsync(UpdateUserRequest request);
        Task<ApiResponse> RemindPasswordAsync(string email);
        Task<ApiResponse> RemindPasswordCompleteAsync(CompleteRemindPasswordRequest request);
    }
}

using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Common.Requests.User;
using DevnotMentor.Api.CustomEntities.Response.UserResponse;

namespace DevnotMentor.Services.Repository.Interfaces
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

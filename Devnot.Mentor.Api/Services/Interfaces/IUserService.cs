using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.OAuth;
using DevnotMentor.Api.CustomEntities.Response.UserResponse;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserLoginResponse>> SignInAsync(OAuthUser oAuthUser);
    }
}

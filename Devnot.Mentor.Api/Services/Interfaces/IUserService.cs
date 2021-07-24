using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Auth;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.Utilities.Security.Token;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<TokenInfo>> SignInAsync(OAuthUser oAuthUser);
        Task<ApiResponse<UserDto>> GetByUserIdAsync(int userId);
    }
}

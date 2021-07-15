using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Response.UserResponse;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserLoginResponse>> GitHubAuth(string id, string name, string login, string avatar);
    }
}

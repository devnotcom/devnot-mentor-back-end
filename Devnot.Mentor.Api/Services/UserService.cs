using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using DevnotMentor.Api.Utilities.Security.Token;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.CustomEntities.Response.UserResponse;
using DevnotMentor.Api.CustomEntities.Dto;

namespace DevnotMentor.Api.Services
{

    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;

        public UserService(
            IMapper mapper,
            IUserRepository userRepository,
            ILoggerRepository loggerRepository,
            ITokenService tokenService,
            IDevnotConfigurationContext devnotConfigurationContext) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            this.tokenService = tokenService;
            this.userRepository = userRepository;
        }

        public async Task<ApiResponse<UserLoginResponse>> GitHubAuth(string githubId, string name, string login, string avatar)
        { //todo: seperate jobs(creating new user - creating token for user)
            var user = await userRepository.GetByGitHubIdOrEmail(githubId);
            if (user == null)
            {
                user = new User()
                {
                    GitHubId = githubId,
                    FullName = name,
                    UserName = login,
                    ProfilePictureUrl = avatar
                };

                user = userRepository.Create(user);
            }

            var tokenInfo = tokenService.CreateToken(user.Id, user.UserName);

            var mappedUser = mapper.Map<User, UserDto>(user);

            return new SuccessApiResponse<UserLoginResponse>(data: new UserLoginResponse(mappedUser, tokenInfo.Token, tokenInfo.ExpiredDate), ResultMessage.Success);
        }
    }
}

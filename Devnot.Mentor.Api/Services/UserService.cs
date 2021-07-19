using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using DevnotMentor.Api.Utilities.Security.Token;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.CustomEntities.OAuth;

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

        private async Task<User> CreateUserForOAuthUserAsync(OAuthUser oAuthUser)
        {
            var user = mapper.Map<User>(oAuthUser);

            switch (oAuthUser.Type)
            {
                case OAuthType.Google:
                    user.GoogleId = oAuthUser.Id;
                    break;
                case OAuthType.GitHub:
                    user.GitHubId = oAuthUser.Id;
                    break;
            }
            /*  todo: 
                if it's github oauth, it can be null. after the registration user must pass a email.
                if it's google oauth, it takes random value */

            return userRepository.Create(user);
        }

        public async Task<ApiResponse<TokenInfo>> SignInAsync(OAuthUser oAuthUser)
        {
            var user = oAuthUser.Type switch
            {
                OAuthType.GitHub => await userRepository.GetByGitHubIdAsync(oAuthUser.Id),
                OAuthType.Google => await userRepository.GetByGoogleIdAsync(oAuthUser.Id),
            };

            if (user == null)
            {
                user = await CreateUserForOAuthUserAsync(oAuthUser);
            }

            return new SuccessApiResponse<TokenInfo>(data: tokenService.CreateToken(user.Id, user.UserName), ResultMessage.Success);
        }
    }
}

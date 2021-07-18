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
using DevnotMentor.Api.CustomEntities.Auth;

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

        private async Task<User> GetOrCreateForOAuthUserAsync(OAuthUser oAuthUser)
        {
            var user = await userRepository.GetByUserNameOrEmailAsync(oAuthUser.IdentifierProperty);

            if (user == null)
            {
                user = new User();
                switch (oAuthUser.Type)
                {
                    case OAuthType.Google:
                        user.GoogleId = oAuthUser.Id;
                        user.Email = oAuthUser.IdentifierProperty;
                        user.UserName = oAuthUser.IdentifierProperty; // todo: after registration, user must select a UserName
                        break;
                    
                    case OAuthType.GitHub:
                        user.GitHubId = oAuthUser.Id;
                        user.UserName = oAuthUser.IdentifierProperty;
                        //user.Email = ""; // todo: after registration, user must select a Email
                        break;
                    
                    default:
                        break;
                }

                user.FullName = oAuthUser.FullName;
                user.ProfilePictureUrl = oAuthUser.ProfilePictureUrl;
                return userRepository.Create(user);
            }

            return user;
        }

        public async Task<ApiResponse<UserLoginResponse>> SignInAsync(OAuthUser oAuthUser)
        {
            User user = await GetOrCreateForOAuthUserAsync(oAuthUser);

            var tokenInfo = tokenService.CreateToken(user.Id, user.UserName);

            var mappedUser = mapper.Map<User, UserDto>(user);

            return new SuccessApiResponse<UserLoginResponse>(data: new UserLoginResponse(mappedUser, tokenInfo.Token, tokenInfo.ExpiredDate), ResultMessage.Success);
        }
    }
}

using System;
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

        private async Task<User> CreateUserForOAuthUserAsync(OAuthUser oAuthUser)
        {
            var checkIsThereAnySimilarUser = await userRepository.AnyByUserNameAsync(oAuthUser.UserName);

            if (checkIsThereAnySimilarUser)
            {
                oAuthUser.SetRandomUsername();
            }

            checkIsThereAnySimilarUser = await userRepository.AnyByEmailAsync(oAuthUser.Email);

            if (checkIsThereAnySimilarUser)
            {
                oAuthUser.Email = null;
                oAuthUser.EmailConfirmed = false;
            }

            var user = mapper.Map<User>(oAuthUser);

            return userRepository.Create(user);
        }

        public async Task<ApiResponse<TokenInfo>> SignInAsync(OAuthUser oAuthUser)
        {
            var user = await oAuthUser.GetUserFromDatabase(userRepository);

            if (user is null)
            {
                user = await CreateUserForOAuthUserAsync(oAuthUser);
            }

            return new SuccessApiResponse<TokenInfo>(data: tokenService.CreateToken(user.Id, user.UserName), ResultMessage.Success);
        }

        public async Task<ApiResponse<UserDto>> GetByUserIdAsync(int userId)
        {
            var user = await userRepository.GetByIdIncludeMenteeMentorAsync(userId);
            return new SuccessApiResponse<UserDto>(data: mapper.Map<UserDto>(user), ResultMessage.Success);
        }
    }
}

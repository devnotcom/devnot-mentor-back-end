using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Services.Interfaces;
using DevnotMentor.Api.Utilities.Security.Token;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Services
{
    //TODO: Aynı username ile kayıt yapılabiliyor

    public class UserService : BaseService, IUserService
    {
        UserRepository repository;
        private ITokenService tokenService;

        public UserService(IOptions<AppSettings> appSettings, IOptions<ResponseMessages> responseMessages, IMapper mapper, MentorDBContext context, ITokenService tokenService) : base(appSettings, responseMessages, mapper, context)
        {
            repository = new UserRepository(context);
            this.tokenService = tokenService;
        }

        public async Task<ApiResponse<User>> Login(LoginModel model)
        {
            var response = new ApiResponse<User>();

            await RunInTry(response, async () =>
            {
                var user = await repository.GetUser(model.UserName, model.Password);

                if (user == null)
                {
                    response.Message = responseMessages.Values["InvalidUserNameOrPassword"];
                }
                else if (!user.UserNameConfirmed.HasValue || !user.UserNameConfirmed.Value)
                {
                    response.Message = responseMessages.Values["UserNameNotValidated"];
                }
                else
                {
                    var tokenData = tokenService.CreateToken(user.Id, user.UserName);

                    user.Token = tokenData.Token;
                    user.TokenExpireDate = tokenData.ExpiredDate;

                    response = new ApiResponse<User> { Success = true, Data = user };
                }
            });

            return response;
        }

        public async Task<ApiResponse<User>> Register(UserModel model)
        {
            var response = new ApiResponse<User>();

            await RunInTry(response, async () =>
            {
                if (!FileHelper.IsValidProfileImage(model.ProfileImage))
                {
                    response.Message = responseMessages.Values["InvalidProfileImage"];
                    return;
                }

                model.ProfileImageUrl = await FileHelper.UploadProfileImage(model.ProfileImage, appSettings);
                var newUser = repository.Create(mapper.Map<User>(model));
                await repository.SaveChangesAsync();

                response.Data = newUser;
                response.Success = true;
            });

            return response;
        }
    }
}

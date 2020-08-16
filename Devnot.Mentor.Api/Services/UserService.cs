using AutoMapper;
using DevnotMentor.Api.Aspects.Autofac.Exception;
using DevnotMentor.Api.Aspects.Autofac.UnitOfWork;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Services.Interfaces;
using DevnotMentor.Api.Utilities.Security.Hash;
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
        private UserRepository userRepository;
        private IHashService hashService;
        private ITokenService tokenService;

        public UserService(IOptions<AppSettings> appSettings, IOptions<ResponseMessages> responseMessages, IMapper mapper, MentorDBContext context, ITokenService tokenService, IHashService hashService) : base(appSettings, responseMessages, mapper, context)
        {
            userRepository = new UserRepository(context);

            this.tokenService = tokenService;
            this.hashService = hashService;
        }

        public async Task<ApiResponse<bool>> ChangePassword(PasswordUpdateModel model)
        {
            var response = new ApiResponse<bool>();

            string hashedLastPassword = hashService.CreateHash(model.LastPassword);

            User currentUser = await userRepository.GetUser(model.UserId, hashedLastPassword);

            if (currentUser == null)
            {
                response.Message = responseMessages.Values["UserNotFound"];
                return response;
            }

            string hashedNewPassword = hashService.CreateHash(model.NewPassword);
            currentUser.Password = hashedNewPassword;

            userRepository.Update(currentUser);

            response.Message = responseMessages.Values["Success"];
            response.Data = true;

            return response;
        }

        [ExceptionHandlingAspect]
        public async Task<ApiResponse<User>> Login(LoginModel model)
        {
            var response = new ApiResponse<User>();

            var user = await userRepository.GetUser(model.UserName, model.Password);

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

                response.Success = true;
                response.Data = user;
            }

            return response;
        }

        [ExceptionHandlingAspect]
        [DevnotUnitOfWorkAspect]
        public async Task<ApiResponse<User>> Register(UserModel model)
        {
            var response = new ApiResponse<User>();

            if (!FileHelper.IsValidProfileImage(model.ProfileImage))
            {
                response.Message = responseMessages.Values["InvalidProfileImage"];
                return response;
            }

            model.ProfileImageUrl = await FileHelper.UploadProfileImage(model.ProfileImage, appSettings);
            model.Password = hashService.CreateHash(model.Password);

            var newUser = userRepository.Create(mapper.Map<User>(model));

            response.Data = newUser;
            response.Success = true;

            return response;
        }
    }
}

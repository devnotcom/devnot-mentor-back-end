using AutoMapper;
using DevnotMentor.Api.Aspects.Autofac.Exception;
using DevnotMentor.Api.Aspects.Autofac.UnitOfWork;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using DevnotMentor.Api.Utilities.Email;
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

    [ExceptionHandlingAspect]
    public class UserService : BaseService, IUserService
    {
        private IUserRepository userRepository;
        private IHashService hashService;
        private ITokenService tokenService;
        private IMailService mailService;

        public UserService(
            IOptions<AppSettings> appSettings,
            IOptions<ResponseMessages> responseMessages,
            IMapper mapper,
            ITokenService tokenService,
            IHashService hashService,
            IMailService mailService,
            IUserRepository userRepository,
            ILoggerRepository loggerRepository
            ) : base(appSettings, responseMessages, mapper, loggerRepository)
        {
            this.tokenService = tokenService;
            this.hashService = hashService;
            this.mailService = mailService;
            this.userRepository = userRepository;
        }

        public async Task<ApiResponse<bool>> ChangePassword(PasswordUpdateModel model)
        {
            var response = new ApiResponse<bool>();

            string hashedLastPassword = hashService.CreateHash(model.LastPassword);

            User currentUser = await userRepository.Get(model.UserId, hashedLastPassword);

            if (currentUser == null)
            {
                response.Message = responseMessages.Values["UserNotFound"];
                return response;
            }

            string hashedNewPassword = hashService.CreateHash(model.NewPassword);
            currentUser.Password = hashedNewPassword;

            userRepository.Update(currentUser);

            response.Message = responseMessages.Values["Success"];
            response.Success = true;
            response.Data = true;

            return response;
        }

        public async Task<ApiResponse<User>> Login(LoginModel model)
        {
            var response = new ApiResponse<User>();

            string hashedPassword = hashService.CreateHash(model.Password);

            var user = await userRepository.Get(model.UserName, hashedPassword);

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

        public async Task<ApiResponse> RemindPassword(string email)
        {
            var response = new ApiResponse();

            if (String.IsNullOrWhiteSpace(email))
            {
                response.Message = responseMessages.Values["InvalidModel"];
                return response;
            }

            var currentUser = await userRepository.GetByEmail(email);

            if (currentUser == null)
            {
                response.Message = responseMessages.Values["UserNotFound"];
                return response;
            }

            currentUser.SecurityKey = Guid.NewGuid();
            currentUser.SecurityKeyExpiryDate = DateTime.Now.AddHours(appSettings.SecurityKeyExpiryFromHours);

            userRepository.Update(currentUser);

            await SendRemindPasswordMail(currentUser);

            response.Success = true;
            return response;
        }

        // TODO: İlerleyen zamanlarda template olarak veri tabanı ya da dosyadan okunulacak.
        private async Task SendRemindPasswordMail(User user)
        {
            var to = new List<string> { user.Email };
            string subject = "Devnot Mentor Programı | Parola Sıfırlama İsteği";
            string remindPasswordUrl = $"{appSettings.UpdatePasswordWebPageUrl}?securityKey={user.SecurityKey}";
            string body = $"Merhaba {user.Name} {user.SurName}, <a href='{remindPasswordUrl}' target='_blank'>buradan</a> parolanızı sıfırlayabilirsiniz.";

            await mailService.SendEmailAsync(to, subject, body);
        }

        public async Task<ApiResponse<User>> Update(UserUpdateModel model)
        {
            var response = new ApiResponse<User>();

            var currentUser = await userRepository.GetById(model.UserId);

            if (model.ProfileImage != null)
            {
                if (!FileHelper.IsValidProfileImage(model.ProfileImage))
                {
                    response.Message = responseMessages.Values["InvalidProfileImage"];
                    return response;
                }

                currentUser.ProfileImageUrl = await FileHelper.UploadProfileImage(model.ProfileImage, appSettings);
            }

            currentUser.Name = model.Name;
            currentUser.SurName = model.SurName;

            userRepository.Update(currentUser);

            response.Data = currentUser;
            response.Message = responseMessages.Values["Success"];
            response.Success = true;

            return response;
        }

        public async Task<ApiResponse> RemindPasswordComplete(RemindPasswordCompleteModel model)
        {
            var apiResponse = new ApiResponse();

            var currentUser = await userRepository.Get(model.SecurityKey);

            if (currentUser == null)
            {
                apiResponse.Message = responseMessages.Values["InvalidSecurityKey"];
                return apiResponse;
            }

            if (currentUser.SecurityKeyExpiryDate < DateTime.Now)
            {
                apiResponse.Message = responseMessages.Values["SecurityKeyExpiryDateAlreadyExpired"];
                return apiResponse;
            }

            currentUser.SecurityKey = null;
            currentUser.Password = hashService.CreateHash(model.Password);

            userRepository.Update(currentUser);

            apiResponse.Success = true;
            apiResponse.Message = responseMessages.Values["Success"];

            return apiResponse;
        }
    }
}

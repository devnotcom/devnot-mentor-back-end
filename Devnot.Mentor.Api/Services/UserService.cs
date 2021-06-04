using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;
using DevnotMentor.Api.Utilities.Email;
using DevnotMentor.Api.Utilities.Security.Hash;
using DevnotMentor.Api.Utilities.Security.Token;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.Utilities.File;

namespace DevnotMentor.Api.Services
{
    //TODO: Aynı username ile kayıt yapılabiliyor

    //[ExceptionHandlingAspect]
    public class UserService : BaseService, IUserService
    {
        private IUserRepository userRepository;
        private IHashService hashService;
        private ITokenService tokenService;
        private IMailService mailService;

        private IFileService fileService;

        public UserService(
            IMapper mapper,
            ITokenService tokenService,
            IHashService hashService,
            IMailService mailService,
            IUserRepository userRepository,
            ILoggerRepository loggerRepository,
            IFileService fileService,
            IDevnotConfigurationContext devnotConfigurationContext) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            this.tokenService = tokenService;
            this.hashService = hashService;
            this.mailService = mailService;
            this.userRepository = userRepository;
            this.fileService = fileService;
        }

        public async Task<ApiResponse<bool>> ChangePassword(PasswordUpdateModel model)
        {
            string hashedLastPassword = hashService.CreateHash(model.LastPassword);

            User currentUser = await userRepository.Get(model.UserId, hashedLastPassword);

            if (currentUser == null)
            {
                return new ErrorApiResponse<bool>(data: false, ResultMessage.NotFoundUser);
            }

            string hashedNewPassword = hashService.CreateHash(model.NewPassword);
            currentUser.Password = hashedNewPassword;

            userRepository.Update(currentUser);

            return new SuccessApiResponse<bool>(data: true, ResultMessage.Success);
        }

        public async Task<ApiResponse<User>> Login(LoginModel model)
        {
            var hashedPassword = hashService.CreateHash(model.Password);

            var user = await userRepository.Get(model.UserName, hashedPassword);

            if (user == null)
            {
                return new ErrorApiResponse<User>(data: null, ResultMessage.InvalidUserNameOrPassword);
            }

            if (!user.UserNameConfirmed.HasValue || !user.UserNameConfirmed.Value)
            {
                return new ErrorApiResponse<User>(data: null, ResultMessage.UserNameIsNotValidated);
            }

            var tokenData = tokenService.CreateToken(user.Id, user.UserName);

            user.Token = tokenData.Token;
            user.TokenExpireDate = tokenData.ExpiredDate;

            return new SuccessApiResponse<User>(data: user, ResultMessage.Success);
        }

        //[DevnotUnitOfWorkAspect]
        public async Task<ApiResponse<User>> Register(UserModel model)
        {
            var checkFileResult = await fileService.InsertProfileImage(model.ProfileImage);

            if (!checkFileResult.IsSuccess)
            {
                return new ErrorApiResponse<User>(data: default, checkFileResult.ErrorMessage);
            }

            model.ProfileImageUrl = checkFileResult.RelativeFilePath;
            model.Password = hashService.CreateHash(model.Password);

            var newUser = userRepository.Create(mapper.Map<User>(model));

            return new SuccessApiResponse<User>(data: newUser, ResultMessage.Success);
        }

        public async Task<ApiResponse> RemindPassword(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return new ErrorApiResponse(ResultMessage.InvalidModel);
            }

            var currentUser = await userRepository.GetByEmail(email);

            if (currentUser == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundUser);
            }

            currentUser.SecurityKey = Guid.NewGuid();
            currentUser.SecurityKeyExpiryDate = DateTime.Now.AddHours(devnotConfigurationContext.SecurityKeyExpiryFromHours);

            userRepository.Update(currentUser);

            await SendRemindPasswordMail(currentUser);

            return new SuccessApiResponse();
        }

        // TODO: İlerleyen zamanlarda template olarak veri tabanı ya da dosyadan okunulacak.
        private async Task SendRemindPasswordMail(User user)
        {
            var to = new List<string> { user.Email };
            string subject = "Devnot Mentor Programı | Parola Sıfırlama İsteği";
            string remindPasswordUrl = $"{devnotConfigurationContext.UpdatePasswordWebPageUrl}?securityKey={user.SecurityKey}";
            string body = $"Merhaba {user.Name} {user.SurName}, <a href='{remindPasswordUrl}' target='_blank'>buradan</a> parolanızı sıfırlayabilirsiniz.";

            await mailService.SendEmailAsync(to, subject, body);
        }

        public async Task<ApiResponse<User>> Update(UserUpdateModel model)
        {
            var currentUser = await userRepository.GetById(model.UserId);

            if (model.ProfileImage != null)
            {
                var checkUploadedImageFileResult = await fileService.InsertProfileImage(model.ProfileImage);

                if (!checkUploadedImageFileResult.IsSuccess)
                {
                    return new ErrorApiResponse<User>(data: default, checkUploadedImageFileResult.ErrorMessage);
                }

                currentUser.ProfileImageUrl = checkUploadedImageFileResult.RelativeFilePath;
            }

            currentUser.Name = model.Name;
            currentUser.SurName = model.SurName;

            userRepository.Update(currentUser);

            return new SuccessApiResponse<User>(currentUser, ResultMessage.Success);
        }

        public async Task<ApiResponse> RemindPasswordComplete(RemindPasswordCompleteModel model)
        {
            var currentUser = await userRepository.Get(model.SecurityKey);

            if (currentUser == null)
            {
                return new ErrorApiResponse(ResultMessage.InvalidSecurityKey);
            }

            if (currentUser.SecurityKeyExpiryDate < DateTime.Now)
            {
                return new ErrorApiResponse(ResultMessage.SecurityKeyExpiryDateAlreadyExpired);
            }

            currentUser.SecurityKey = null;
            currentUser.Password = hashService.CreateHash(model.Password);

            userRepository.Update(currentUser);

            return new SuccessApiResponse(ResultMessage.Success);
        }
    }
}

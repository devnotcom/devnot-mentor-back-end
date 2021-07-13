using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
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
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.CustomEntities.Request.UserRequest;
using DevnotMentor.Api.CustomEntities.Response.UserResponse;
using DevnotMentor.Api.Utilities.File;

namespace DevnotMentor.Api.Services
{
    //TODO: Aynı username ile kayıt yapılabiliyor

    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IHashService hashService;
        private readonly ITokenService tokenService;
        private readonly IMailService mailService;

        private readonly IFileService fileService;

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

        public async Task<ApiResponse> ChangePassword(UpdatePasswordRequest request)
        {
            string hashedLastPassword = hashService.CreateHash(request.LastPassword);

            var currentUser = await userRepository.Get(request.UserId, hashedLastPassword);

            if (currentUser == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundUser);
            }

            currentUser.Password = hashService.CreateHash(request.NewPassword);

            userRepository.Update(currentUser);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        public async Task<ApiResponse<UserLoginResponse>> Login(UserLoginRequest request)
        {
            var hashedPassword = hashService.CreateHash(request.Password);

            var user = await userRepository.Get(request.UserName, hashedPassword);

            if (user == null)
            {
                return new ErrorApiResponse<UserLoginResponse>(data: null, ResultMessage.InvalidUserNameOrPassword);
            }

            if (!user.UserNameConfirmed.HasValue || !user.UserNameConfirmed.Value)
            {
                return new ErrorApiResponse<UserLoginResponse>(data: null, ResultMessage.UserNameIsNotValidated);
            }

            var tokenData = tokenService.CreateToken(user.Id, user.UserName);

            user.Token = tokenData.Token;
            user.TokenExpireDate = tokenData.ExpiredDate;

            var mappedUser = mapper.Map<User, UserDto>(user);

            var loginResponse = new UserLoginResponse(mappedUser, user.Token, user.TokenExpireDate);

            return new SuccessApiResponse<UserLoginResponse>(data: loginResponse, ResultMessage.Success);
        }

        public async Task<ApiResponse> Register(RegisterUserRequest request)
        {
            var checkFileResult = await fileService.InsertProfileImage(request.ProfileImage);

            if (!checkFileResult.IsSuccess)
            {
                return new ErrorApiResponse<User>(data: default, checkFileResult.ErrorMessage);
            }

            request.ProfileImageUrl = checkFileResult.RelativeFilePath;
            request.Password = hashService.CreateHash(request.Password);

            userRepository.Create(mapper.Map<User>(request));

            return new SuccessApiResponse(ResultMessage.Success);
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

            return new SuccessApiResponse(ResultMessage.Success);
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

        public async Task<ApiResponse> Update(UpdateUserRequest request)
        {
            var currentUser = await userRepository.GetById(request.UserId);

            if (request.ProfileImage != null)
            {
                var checkUploadedImageFileResult = await fileService.InsertProfileImage(request.ProfileImage);

                if (!checkUploadedImageFileResult.IsSuccess)
                {
                    return new ErrorApiResponse<User>(data: default, checkUploadedImageFileResult.ErrorMessage);
                }

                currentUser.ProfileImageUrl = checkUploadedImageFileResult.RelativeFilePath;
            }

            currentUser.Name = request.Name;
            currentUser.SurName = request.SurName;

            userRepository.Update(currentUser);

            return new SuccessApiResponse(ResultMessage.Success);
        }

        public async Task<ApiResponse> RemindPasswordComplete(CompleteRemindPasswordRequest request)
        {
            var currentUser = await userRepository.Get(request.SecurityKey);

            if (currentUser == null)
            {
                return new ErrorApiResponse(ResultMessage.InvalidSecurityKey);
            }

            if (currentUser.SecurityKeyExpiryDate < DateTime.Now)
            {
                return new ErrorApiResponse(ResultMessage.SecurityKeyExpiryDateAlreadyExpired);
            }

            currentUser.SecurityKey = null;
            currentUser.Password = hashService.CreateHash(request.Password);

            userRepository.Update(currentUser);

            return new SuccessApiResponse(ResultMessage.Success);
        }
    }
}

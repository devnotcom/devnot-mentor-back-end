using AutoMapper;
using DevnotMentor.Common;
using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Services.Repository.Interfaces;
using DevnotMentor.Services.Utilities.Email;
using DevnotMentor.Services.Utilities.Security.Hash;
using DevnotMentor.Services.Utilities.Security.Token;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.User;
using DevnotMentor.Api.CustomEntities.Response.UserResponse;
using DevnotMentor.Services.Utilities.File;

namespace DevnotMentor.Services.Repository
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
            ILogRepository loggerRepository,
            IFileService fileService,
            IDevnotConfigurationContext devnotConfigurationContext) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            this.tokenService = tokenService;
            this.hashService = hashService;
            this.mailService = mailService;
            this.userRepository = userRepository;
            this.fileService = fileService;
        }

        public async Task<ApiResponse> ChangePasswordAsync(UpdatePasswordRequest request)
        {
            string hashedLastPassword = hashService.CreateHash(request.LastPassword);

            var currentUser = await userRepository.GetAsync(request.UserId, hashedLastPassword);

            if (currentUser == null)
            {
                return new ErrorApiResponse(ResponseStatus.NotFound, ResultMessage.NotFoundUser);
            }

            currentUser.Password = hashService.CreateHash(request.NewPassword);

            userRepository.Update(currentUser);

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse<UserLoginResponse>> LoginAsync(UserLoginRequest request)
        {
            var hashedPassword = hashService.CreateHash(request.Password);

            var user = await userRepository.GetAsync(request.UserName, hashedPassword);

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

            var mappedUser = mapper.Map<User, UserDTO>(user);

            var loginResponse = new UserLoginResponse(mappedUser, user.Token, user.TokenExpireDate);

            return new SuccessApiResponse<UserLoginResponse>(loginResponse);
        }

        public async Task<ApiResponse> RegisterAsync(RegisterUserRequest request)
        {
            var checkFileResult = await fileService.InsertProfileImageAsync(request.ProfileImage);

            if (!checkFileResult.IsSuccess)
            {
                return new ErrorApiResponse<User>(data: default, checkFileResult.ErrorMessage);
            }

            request.ProfileImageUrl = checkFileResult.RelativeFilePath;
            request.Password = hashService.CreateHash(request.Password);

            userRepository.Create(mapper.Map<User>(request));

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse> RemindPasswordAsync(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return new ErrorApiResponse(ResultMessage.InvalidModel);
            }

            var currentUser = await userRepository.GetByEmailAsync(email);

            if (currentUser == null)
            {
                return new ErrorApiResponse(ResponseStatus.NotFound, ResultMessage.NotFoundUser);
            }

            currentUser.SecurityKey = Guid.NewGuid();
            currentUser.SecurityKeyExpiryDate = DateTime.Now.AddHours(devnotConfigurationContext.SecurityKeyExpiryFromHours);

            userRepository.Update(currentUser);

            await SendRemindPasswordMailAsync(currentUser);

            return new SuccessApiResponse();
        }

        // TODO: İlerleyen zamanlarda template olarak veri tabanı ya da dosyadan okunulacak.
        private async Task SendRemindPasswordMailAsync(User user)
        {
            var to = new List<string> { user.Email };
            string subject = "Devnot Mentor Programı | Parola Sıfırlama İsteği";
            string remindPasswordUrl = $"{devnotConfigurationContext.UpdatePasswordWebPageUrl}?securityKey={user.SecurityKey}";
            string body = $"Merhaba {user.Name} {user.SurName}, <a href='{remindPasswordUrl}' target='_blank'>buradan</a> parolanızı sıfırlayabilirsiniz.";

            await mailService.SendEmailAsync(to, subject, body);
        }

        public async Task<ApiResponse> UpdateAsync(UpdateUserRequest request)
        {
            var currentUser = await userRepository.GetByIdAsync(request.UserId);

            if (request.ProfileImage != null)
            {
                var checkUploadedImageFileResult = await fileService.InsertProfileImageAsync(request.ProfileImage);

                if (!checkUploadedImageFileResult.IsSuccess)
                {
                    return new ErrorApiResponse<User>(data: default, checkUploadedImageFileResult.ErrorMessage);
                }

                currentUser.ProfileImageUrl = checkUploadedImageFileResult.RelativeFilePath;
            }

            currentUser.Name = request.Name;
            currentUser.SurName = request.SurName;

            userRepository.Update(currentUser);

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse> RemindPasswordCompleteAsync(CompleteRemindPasswordRequest request)
        {
            var currentUser = await userRepository.GetAsync(request.SecurityKey);

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

            return new SuccessApiResponse();
        }
    }
}

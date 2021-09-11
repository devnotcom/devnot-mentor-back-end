using AutoMapper;
using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Business.Services.Interfaces;
using DevnotMentor.Business.Utilities.Email;
using DevnotMentor.Business.Utilities.Security.Hash;
using DevnotMentor.Business.Utilities.Security.Token;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Configuration.Context;
using DevnotMentor.Common.DTO;
using DevnotMentor.Common.Requests.User;
using DevnotMentor.Business.Utilities.File;
using DevnotMentor.Common.Responses.User;

namespace DevnotMentor.Business.Services
{
    //TODO: Aynı username ile kayıt yapılabiliyor

    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly IMailService _mailService;
        private readonly IFileService _fileService;

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
            this._tokenService = tokenService;
            this._hashService = hashService;
            this._mailService = mailService;
            this._userRepository = userRepository;
            this._fileService = fileService;
        }

        public async Task<ApiResponse> ChangePasswordAsync(UpdatePasswordRequest request)
        {
            string hashedLastPassword = _hashService.CreateHash(request.LastPassword);

            var currentUser = await _userRepository.GetAsync(request.UserId, hashedLastPassword);

            if (currentUser == null)
            {
                return new ErrorApiResponse(ResponseStatus.NotFound, ResultMessage.NotFoundUser);
            }

            currentUser.Password = _hashService.CreateHash(request.NewPassword);

            _userRepository.Update(currentUser);

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse<UserLoginResponse>> LoginAsync(UserLoginRequest request)
        {
            var hashedPassword = _hashService.CreateHash(request.Password);

            var user = await _userRepository.GetAsync(request.UserName, hashedPassword);

            if (user == null)
            {
                return new ErrorApiResponse<UserLoginResponse>(data: null, ResultMessage.InvalidUserNameOrPassword);
            }

            if (!user.UserNameConfirmed.HasValue || !user.UserNameConfirmed.Value)
            {
                return new ErrorApiResponse<UserLoginResponse>(data: null, ResultMessage.UserNameIsNotValidated);
            }

            var tokenData = _tokenService.CreateToken(user.Id, user.UserName);

            user.Token = tokenData.Token;
            user.TokenExpireDate = tokenData.ExpiredDate;

            var mappedUser = _mapper.Map<User, UserDTO>(user);

            var loginResponse = new UserLoginResponse(mappedUser, user.Token, user.TokenExpireDate);

            return new SuccessApiResponse<UserLoginResponse>(loginResponse);
        }

        public async Task<ApiResponse> RegisterAsync(RegisterUserRequest request)
        {
            var checkFileResult = await _fileService.InsertProfileImageAsync(request.ProfileImage);

            if (!checkFileResult.IsSuccess)
            {
                return new ErrorApiResponse<User>(data: default, checkFileResult.ErrorMessage);
            }

            request.ProfileImageUrl = checkFileResult.RelativeFilePath;
            request.Password = _hashService.CreateHash(request.Password);

            _userRepository.Create(_mapper.Map<User>(request));

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse> RemindPasswordAsync(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return new ErrorApiResponse(ResultMessage.InvalidModel);
            }

            var currentUser = await _userRepository.GetByEmailAsync(email);

            if (currentUser == null)
            {
                return new ErrorApiResponse(ResponseStatus.NotFound, ResultMessage.NotFoundUser);
            }

            currentUser.SecurityKey = Guid.NewGuid();
            currentUser.SecurityKeyExpiryDate = DateTime.Now.AddHours(_devnotConfigurationContext.SecurityKeyExpiryFromHours);

            _userRepository.Update(currentUser);

            await SendRemindPasswordMailAsync(currentUser);

            return new SuccessApiResponse();
        }

        // TODO: İlerleyen zamanlarda template olarak veri tabanı ya da dosyadan okunulacak.
        private async Task SendRemindPasswordMailAsync(User user)
        {
            var to = new List<string> { user.Email };
            string subject = "Devnot Mentor Programı | Parola Sıfırlama İsteği";
            string remindPasswordUrl = $"{_devnotConfigurationContext.UpdatePasswordWebPageUrl}?securityKey={user.SecurityKey}";
            string body = $"Merhaba {user.Name} {user.SurName}, <a href='{remindPasswordUrl}' target='_blank'>buradan</a> parolanızı sıfırlayabilirsiniz.";

            await _mailService.SendEmailAsync(to, subject, body);
        }

        public async Task<ApiResponse> UpdateAsync(UpdateUserRequest request)
        {
            var currentUser = await _userRepository.GetByIdAsync(request.UserId);

            if (request.ProfileImage != null)
            {
                var checkUploadedImageFileResult = await _fileService.InsertProfileImageAsync(request.ProfileImage);

                if (!checkUploadedImageFileResult.IsSuccess)
                {
                    return new ErrorApiResponse<User>(data: default, checkUploadedImageFileResult.ErrorMessage);
                }

                currentUser.ProfileImageUrl = checkUploadedImageFileResult.RelativeFilePath;
            }

            currentUser.Name = request.Name;
            currentUser.SurName = request.SurName;

            _userRepository.Update(currentUser);

            return new SuccessApiResponse();
        }

        public async Task<ApiResponse> RemindPasswordCompleteAsync(CompleteRemindPasswordRequest request)
        {
            var currentUser = await _userRepository.GetAsync(request.SecurityKey);

            if (currentUser == null)
            {
                return new ErrorApiResponse(ResultMessage.InvalidSecurityKey);
            }

            if (currentUser.SecurityKeyExpiryDate < DateTime.Now)
            {
                return new ErrorApiResponse(ResultMessage.SecurityKeyExpiryDateAlreadyExpired);
            }

            currentUser.SecurityKey = null;
            currentUser.Password = _hashService.CreateHash(request.Password);

            _userRepository.Update(currentUser);

            return new SuccessApiResponse();
        }
    }
}

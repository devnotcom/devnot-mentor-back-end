using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Services.Interfaces;
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

        public UserService(IOptions<AppSettings> appSettings, IOptions<ResponseMessages> responseMessages, IMapper mapper, MentorDBContext context) :base(appSettings, responseMessages, mapper, context)
        {
            repository = new UserRepository(context);
        }

        public async Task<ApiResponse<User>> Login(LoginModel model)
        {
            var response = new ApiResponse<User>();

            await RunInTry(response, async() => {
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
                    user.Token = GenerateToken(user.UserName);
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
                if(!FileHelper.IsValidProfileImage(model.ProfileImage))
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

        public string GenerateToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, userName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(appSettings.SecretExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //public async Task<bool> IsValidToken(string userName, string token)
        //{
        //    return new Task<bool>();
        //}
    }
}

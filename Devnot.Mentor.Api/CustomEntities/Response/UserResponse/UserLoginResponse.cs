using System;
using DevnotMentor.Api.CustomEntities.Dto;

namespace DevnotMentor.Api.CustomEntities.Response.UserResponse
{
    public class UserLoginResponse
    {
        public UserLoginResponse(UserDto user, string token, DateTime? tokenExpiryDate)
        {
            User = user;
            Token = token;
            TokenExpiryDate = tokenExpiryDate;
        }

        public UserDto User { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpiryDate { get; set; }
    }
}

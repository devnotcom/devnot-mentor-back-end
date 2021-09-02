using System;
using DevnotMentor.Common.DTO;

namespace DevnotMentor.Api.CustomEntities.Response.UserResponse
{
    public class UserLoginResponse
    {
        public UserLoginResponse(UserDTO user, string token, DateTime? tokenExpiryDate)
        {
            User = user;
            Token = token;
            TokenExpiryDate = tokenExpiryDate;
        }

        public UserDTO User { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpiryDate { get; set; }
    }
}

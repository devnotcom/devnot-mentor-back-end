using System;
using DevnotMentor.Common.DTO;

namespace DevnotMentor.Common.Responses.User
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

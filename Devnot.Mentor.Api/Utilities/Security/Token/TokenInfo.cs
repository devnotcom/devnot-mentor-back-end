using System;

namespace DevnotMentor.Api.Utilities.Security.Token
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}

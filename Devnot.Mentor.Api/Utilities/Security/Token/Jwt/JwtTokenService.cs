using DevnotMentor.Api.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Utilities.Security.Token
{
    public class JwtTokenService : ITokenService
    {
        public AppSettings AppSettings { get; set; }

        public JwtTokenService(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public TokenInfo CreateToken(int userId, string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, userName),
                    new Claim("UserId",userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(AppSettings.SecretExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            tokenHandler.WriteToken(token);

            var tokenInfo = new TokenInfo();
            tokenInfo.Token = tokenHandler.WriteToken(token);
            tokenInfo.ExpiredDate = DateTime.Now.AddMinutes(AppSettings.SecretExpirationInMinutes);

            return tokenInfo;
        }

        public IEnumerable<Claim> ReadToken(string token)
        {
            var securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return securityToken.Claims;
        }
    }
}

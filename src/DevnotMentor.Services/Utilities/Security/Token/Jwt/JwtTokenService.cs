using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevnotMentor.Configurations.Context;
using Microsoft.IdentityModel.Tokens;

namespace DevnotMentor.Services.Utilities.Security.Token.Jwt
{
    public class JwtTokenService : ITokenService
    {
        private readonly IDevnotConfigurationContext devnotConfigurationContext;

        public JwtTokenService(IDevnotConfigurationContext devnotConfigurationContext)
        {
            this.devnotConfigurationContext = devnotConfigurationContext;
        }

        public TokenInfo CreateToken(int userId, string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(devnotConfigurationContext.JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, userName),
                    new Claim("UserId",userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(devnotConfigurationContext.JwtSecretExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = devnotConfigurationContext.JwtValidIssuer,
                Audience = devnotConfigurationContext.JwtValidAudience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            tokenHandler.WriteToken(token);

            return new TokenInfo
            {
                Token = tokenHandler.WriteToken(token),
                ExpiredDate = DateTime.Now.AddMinutes(devnotConfigurationContext.JwtSecretExpirationInMinutes)
            };
        }

        public ResolveTokenResult ResolveToken(string token)
        {
            var resolveTokenResult = new ResolveTokenResult();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(devnotConfigurationContext.JwtSecret);
            var securityKey = new SymmetricSecurityKey(key);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = devnotConfigurationContext.JwtValidAudience,
                    ValidIssuer = devnotConfigurationContext.JwtValidIssuer,
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);

                resolveTokenResult.ExpiryDate = validatedToken.ValidTo;
                resolveTokenResult.IsValid = true;
                resolveTokenResult.Claims = tokenHandler.ReadJwtToken(token).Claims;
            }

            catch (Exception ex)
            {
                resolveTokenResult.IsValid = false;
                resolveTokenResult.ErrorMessage = ex.Message;
            }

            return resolveTokenResult;
        }
    }
}

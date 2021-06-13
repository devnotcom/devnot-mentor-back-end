using System.Collections.Generic;
using System.Security.Claims;

namespace DevnotMentor.Api.Utilities.Security.Token
{
    public interface ITokenService
    {
        /// <summary>
        /// Generate JWT token with user id and user name.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        TokenInfo CreateToken(int userId, string userName);
        /// <summary>
        /// Read specified token. It returns token status. It is valid or is not valid. Return value that contains token status, expiry date and claims.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ResolveTokenResult ResolveToken(string token);
    }
}

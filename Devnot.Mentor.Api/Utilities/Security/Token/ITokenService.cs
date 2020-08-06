using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Utilities.Security.Token
{
    public interface ITokenService
    {
        TokenInfo CreateToken(int userId, string userName);
        IEnumerable<Claim> ReadToken(string token);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Helpers.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetUserId(this IEnumerable<Claim> claims)
        {
            Claim userIdClaim = claims.Where(i => i.Type == "UserId").First();
            return int.Parse(userIdClaim.Value);
        }
    }
}

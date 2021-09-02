using System.Linq;
using System.Security.Claims;

namespace DevnotMentor.WebAPI.Helpers.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal claimsPrincipal)
        {
            Claim userIdClaim = claimsPrincipal.Claims.Where(i => i.Type == "UserId").FirstOrDefault();
            return userIdClaim == default ? default : int.Parse(userIdClaim.Value);
        }
    }
}

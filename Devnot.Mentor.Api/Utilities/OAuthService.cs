using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Auth;
using DevnotMentor.Api.CustomEntities.Auth.Response;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DevnotMentor.Api.Utilities
{
    public static class OAuthService
    {
        public static async Task<OAuthGitHubUser> GetOAuthGitHubUserAsync(OAuthCreatingTicketContext creatinTicketContext)
        {
            var authGitHubResponse = await OAuthServiceResponse.GetUserPublicInformationsAsync<OAuthGitHubResponse>(creatinTicketContext);
            authGitHubResponse.Emails = await OAuthServiceResponse.GetGitHubEmailsAsync(creatinTicketContext);
            
            return authGitHubResponse.MapToOAuthGitHubUser();
        }

        public static async Task<OAuthGoogleUser> GetOAuthGoogleUserAsync(OAuthCreatingTicketContext creatinTicketContext)
        {
            var authGoogleResponse = await OAuthServiceResponse.GetUserPublicInformationsAsync<OAuthGoogleResponse>(creatinTicketContext);
            return authGoogleResponse.MapToOAuthGoogleUser();
        }

        public static async Task SignInAsync(OAuthUser oAuthUser, HttpContext httpContext)
        {
            var userService = httpContext.RequestServices.GetService<IUserService>();
            var signInResponse = await userService.SignInAsync(oAuthUser);
            if (signInResponse.Success)
            {
                httpContext.Response.Headers.Add("auth-token", signInResponse.Data.Token);
                httpContext.Response.Headers.Add("auth-token-expiry-date", signInResponse.Data.ExpiryDate.ToString());
            }
            else
            {
                httpContext.Response.StatusCode = 500;
            }
        }
    }
}
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using DevnotMentor.Api.CustomEntities.Auth;
using DevnotMentor.Api.CustomEntities.Auth.Response;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DevnotMentor.Api.Utilities
{
    public static class OAuthService
    {
        public static async Task<OAuthGitHubUser> GetOAuthGitHubUserAsync(OAuthCreatingTicketContext ctx)
        {
            var authGitHubResponse = await GetOAuthUserAsync<OAuthGitHubResponse>(ctx);
            return authGitHubResponse.MapToOAuthGitHubUser();
        }

        public static async Task<OAuthGoogleUser> GetOAuthGoogleUserAsync(OAuthCreatingTicketContext ctx)
        {
            var authGoogleResponse = await GetOAuthUserAsync<OAuthGoogleResponse>(ctx);
            return authGoogleResponse.MapToOAuthGoogleUser();
        }

        public static async Task<TOAuthResponse> GetOAuthUserAsync<TOAuthResponse>(OAuthCreatingTicketContext ctx)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await ctx.Backchannel.SendAsync(request, ctx.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<TOAuthResponse>(await response.Content.ReadAsStringAsync());
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
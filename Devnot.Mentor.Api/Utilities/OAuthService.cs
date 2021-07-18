using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using DevnotMentor.Api.CustomEntities.Auth;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DevnotMentor.Api.Utilities
{
    public static class OAuthService
    {
        public static async Task<OAuthUser> GetOAuthUserAsync(OAuthType oAuthType, OAuthCreatingTicketContext ctx)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await ctx.Backchannel.SendAsync(request, ctx.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            if (oAuthType == OAuthType.Google)
            {
                var googleResponse = JsonConvert.DeserializeObject<GoogleResponse>(responseContent);
                return new OAuthUser(googleResponse.id, googleResponse.email, googleResponse.name, googleResponse.picture, oAuthType);
            }

            var gitHubResponse = JsonConvert.DeserializeObject<GitHubResponse>(responseContent);
            return new OAuthUser(gitHubResponse.id, gitHubResponse.login, gitHubResponse.name, gitHubResponse.avatar_url, oAuthType);
        }

        public static async Task SignInAsync(OAuthUser oAuthUser, HttpContext httpContext)
        {
            var userService = httpContext.RequestServices.GetService<IUserService>();
            var signInResponse = await userService.SignInAsync(oAuthUser);
            if (signInResponse.Success)
            {
                httpContext.Response.Headers.Add("auth-token", signInResponse.Data.Token);
                httpContext.Response.Headers.Add("auth-token-expiry-date", signInResponse.Data.TokenExpiryDate.ToString());
            }
            else
            {
                httpContext.Response.StatusCode = 500;
            }
        }
    }
}
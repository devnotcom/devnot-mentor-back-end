using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using DevnotMentor.Api.CustomEntities.OAuth;
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

            var oauthResponse = JsonConvert.DeserializeObject<OAuthResponse>(await response.Content.ReadAsStringAsync());

            return new OAuthUser(oauthResponse, oAuthType);
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
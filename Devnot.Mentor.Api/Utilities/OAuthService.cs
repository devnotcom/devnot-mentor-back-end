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
using System.Collections.Generic;

namespace DevnotMentor.Api.Utilities
{
    public static class OAuthService
    {
        private static async Task<string> GetResponseContentAsStringAsync(HttpRequestMessage request, OAuthCreatingTicketContext ctx)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await ctx.Backchannel.SendAsync(request, ctx.HttpContext.RequestAborted);

            return await response.Content.ReadAsStringAsync();
        }

        private static async Task<TOAuthResponse> GetUserPublicInformationsAsync<TOAuthResponse>(OAuthCreatingTicketContext ctx)
        {
            var publicInfoRequest = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
            var publicInfoResponse = await GetResponseContentAsStringAsync(publicInfoRequest, ctx);

            return JsonConvert.DeserializeObject<TOAuthResponse>(publicInfoResponse);
        }

        private static async Task<List<OAuthGitHubEmailResponse>> GetGitHubEmailsAsync(OAuthCreatingTicketContext ctx)
        {
            var emailRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
            var emailResponse = await GetResponseContentAsStringAsync(emailRequest, ctx);

            return JsonConvert.DeserializeObject<List<OAuthGitHubEmailResponse>>(emailResponse);
        }

        public static async Task<OAuthGitHubUser> GetOAuthGitHubUserAsync(OAuthCreatingTicketContext ctx)
        {
            var authGitHubResponse = await GetUserPublicInformationsAsync<OAuthGitHubResponse>(ctx);
            authGitHubResponse.Emails = await GetGitHubEmailsAsync(ctx);
            
            return authGitHubResponse.MapToOAuthGitHubUser();
        }

        public static async Task<OAuthGoogleUser> GetOAuthGoogleUserAsync(OAuthCreatingTicketContext ctx)
        {
            var authGoogleResponse = await GetUserPublicInformationsAsync<OAuthGoogleResponse>(ctx);
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
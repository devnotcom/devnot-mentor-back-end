using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using DevnotMentor.Api.CustomEntities.Auth.Response;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Collections.Generic;

namespace DevnotMentor.Api.Utilities
{
    public static class OAuthResponseService
    {
        /// <summary>
        /// Common method to get OAuth response as string
        /// </summary>
        /// <param name="requestWithMethodAndURI"></param>
        /// <param name="creatingTicketContext"></param>
        /// <returns><see cref="string"/></returns>
        private static async Task<string> GetResponseContentAsStringAsync(HttpRequestMessage requestWithMethodAndURI, OAuthCreatingTicketContext creatingTicketContext)
        {
            requestWithMethodAndURI.Headers.Authorization = new AuthenticationHeaderValue("Bearer", creatingTicketContext.AccessToken);
            requestWithMethodAndURI.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await creatingTicketContext.Backchannel.SendAsync(requestWithMethodAndURI, creatingTicketContext.HttpContext.RequestAborted);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Common method to get user public informations from OAuth providers
        /// </summary>
        /// <param name="creatingTicketContext"></param>
        /// <typeparam name="TOAuthResponse">Generic response</typeparam>
        /// <returns>Deserialized generic response. Example: <see cref="OAuthGitHubResponse"/>, <see cref="OAuthGoogleResponse"/></returns>
        public static async Task<TOAuthResponse> GetUserPublicInformationsAsync<TOAuthResponse>(OAuthCreatingTicketContext creatingTicketContext)
        {
            var publicInfoRequest = new HttpRequestMessage(HttpMethod.Get, creatingTicketContext.Options.UserInformationEndpoint);
            var publicInfoResponse = await GetResponseContentAsStringAsync(publicInfoRequest, creatingTicketContext);

            return JsonConvert.DeserializeObject<TOAuthResponse>(publicInfoResponse);
        }

        /// <summary>
        /// Get user emails from GitHub OAuth
        /// </summary>
        /// <param name="creatingTicketContext"></param>
        /// <returns>A list of <see cref="OAuthGitHubEmailResponse"/></returns>
        public static async Task<List<OAuthGitHubEmailResponse>> GetGitHubEmailsAsync(OAuthCreatingTicketContext creatingTicketContext)
        {
            var emailRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
            var emailResponse = await GetResponseContentAsStringAsync(emailRequest, creatingTicketContext);

            return JsonConvert.DeserializeObject<List<OAuthGitHubEmailResponse>>(emailResponse);
        }
    }
}
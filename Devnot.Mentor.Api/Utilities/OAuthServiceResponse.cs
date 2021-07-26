using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using DevnotMentor.Api.CustomEntities.Auth.Response;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Collections.Generic;

namespace DevnotMentor.Api.Utilities
{
    public static class OAuthServiceResponse
    {
        public static async Task<string> GetResponseContentAsStringAsync(HttpRequestMessage requestWithMethodAndURI, OAuthCreatingTicketContext creatinTicketContext)
        {
            requestWithMethodAndURI.Headers.Authorization = new AuthenticationHeaderValue("Bearer", creatinTicketContext.AccessToken);
            requestWithMethodAndURI.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await creatinTicketContext.Backchannel.SendAsync(requestWithMethodAndURI, creatinTicketContext.HttpContext.RequestAborted);

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<TOAuthResponse> GetUserPublicInformationsAsync<TOAuthResponse>(OAuthCreatingTicketContext creatinTicketContext)
        {
            var publicInfoRequest = new HttpRequestMessage(HttpMethod.Get, creatinTicketContext.Options.UserInformationEndpoint);
            var publicInfoResponse = await GetResponseContentAsStringAsync(publicInfoRequest, creatinTicketContext);

            return JsonConvert.DeserializeObject<TOAuthResponse>(publicInfoResponse);
        }

        public static async Task<List<OAuthGitHubEmailResponse>> GetGitHubEmailsAsync(OAuthCreatingTicketContext creatinTicketContext)
        {
            var emailRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
            var emailResponse = await GetResponseContentAsStringAsync(emailRequest, creatinTicketContext);

            return JsonConvert.DeserializeObject<List<OAuthGitHubEmailResponse>>(emailResponse);
        }
    }
}
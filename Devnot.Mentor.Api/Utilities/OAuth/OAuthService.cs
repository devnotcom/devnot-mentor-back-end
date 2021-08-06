using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Auth;
using DevnotMentor.Api.CustomEntities.Auth.Response;
using Newtonsoft.Json;

namespace DevnotMentor.Api.Utilities.OAuth
{
    public static class OAuthService
    {
        /// <summary>
        /// Get provider response as deserialized
        /// </summary>
        /// <param name="endpoint"><see cref="OAuthDefaults"></param>
        /// <param name="accessToken">Acces token which is coming from Client</param>
        /// <typeparam name="TOAuthResponse">Generic responses</typeparam>
        /// <returns>Deserialized generic response. Example: <see cref="OAuthGitHubResponse"/>, <see cref="OAuthGoogleResponse"/></returns>
        private static async Task<TOAuthResponse> GetOAuthResponseAsDeserializedAsync<TOAuthResponse>(string endpoint, string accessToken)
        {
            HttpClient httpClient = new HttpClient();

            HttpRequestMessage oauthRequest = new HttpRequestMessage(HttpMethod.Get, endpoint);
            oauthRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            oauthRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            oauthRequest.Headers.Add("User-Agent", OAuthDefaults.UserAgent);

            HttpResponseMessage oauthResponse = await httpClient.SendAsync(oauthRequest);

            oauthRequest.Dispose();
            httpClient.Dispose();

            var oauthResponseContentAsString = await oauthResponse.Content.ReadAsStringAsync();

            return oauthResponse.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<TOAuthResponse>(oauthResponseContentAsString)
                : throw new System.Exception(oauthResponseContentAsString); // todo: return meaningful message for client
        }

        public static async Task<OAuthGitHubUser> GetOAuthGitHubUserAsync(string accessToken)
        {
            var authGitHubResponse = await GetOAuthResponseAsDeserializedAsync<OAuthGitHubResponse>(
                OAuthDefaults.GitHubUserInformationEndpoint,
                accessToken);

            authGitHubResponse.Emails = await GetOAuthResponseAsDeserializedAsync<List<OAuthGitHubEmailResponse>>(
                OAuthDefaults.GitHubUserEmailEndpoint,
                accessToken);

            return authGitHubResponse.MapToOAuthGitHubUser();
        }

        public static async Task<OAuthGoogleUser> GetOAuthGoogleUserAsync(string accessToken)
        {
            var authGoogleResponse = await GetOAuthResponseAsDeserializedAsync<OAuthGoogleResponse>(
                OAuthDefaults.GoogleUserInformationEndpoint,
                accessToken);

            return authGoogleResponse.MapToOAuthGoogleUser();
        }
    }
}
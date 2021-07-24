using System;

namespace DevnotMentor.Api.CustomEntities.Auth.Response
{
    public class OAuthGitHubResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string avatar_url { get; set; }

        public OAuthGitHubUser MapToOAuthGitHubUser()
        {
            return new OAuthGitHubUser()
            {
                Email = email,
                Id = id,
                FullName = name,
                ProfilePictureUrl = avatar_url,
                UserName = login,
                EmailConfirmed = (!String.IsNullOrEmpty(email))
            };
        }
    }
}

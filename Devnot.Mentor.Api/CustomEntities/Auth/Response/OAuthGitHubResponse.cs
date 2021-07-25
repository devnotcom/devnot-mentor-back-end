using System.Collections.Generic;

namespace DevnotMentor.Api.CustomEntities.Auth.Response
{
    public class OAuthGitHubResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string avatar_url { get; set; }
        public List<OAuthGitHubEmailResponse> Emails { get; set; }

        public OAuthGitHubUser MapToOAuthGitHubUser()
        {
            var primaryEmail = Emails.Find(x => x.primary == true);
            
            return new OAuthGitHubUser()
            {
                Email = primaryEmail?.email,
                Id = id,
                FullName = name,
                ProfilePictureUrl = avatar_url,
                UserName = login,
                EmailConfirmed = primaryEmail is null ? false : primaryEmail.verified
            };
        }
    }
}

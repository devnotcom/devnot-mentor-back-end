namespace DevnotMentor.Api.CustomEntities.Auth.Response
{
    public class OAuthGitHubEmailResponse
    {
        public string email { get; set; }
        public bool primary { get; set; }
        public bool verified { get; set; }
    }
}
namespace DevnotMentor.Api.Utilities.OAuth
{
    public static class OAuthDefaults
    {
        public static string UserAgent = "DEVNOT MENTOR";
        
        public static string GoogleUserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
        
        public static string GitHubUserInformationEndpoint = "https://api.github.com/user";
        public static string GitHubUserEmailEndpoint = "https://api.github.com/user/emails";
    }
}
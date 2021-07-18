namespace DevnotMentor.Api.CustomEntities.Auth
{
    public class OAuthUser
    {
        public OAuthUser(string id, string uniqueByProvider, string fullName, string profilePictureUrl, OAuthType type)
        {
            Id = id;
            UniqueByProvider = uniqueByProvider;
            FullName = fullName;
            ProfilePictureUrl = profilePictureUrl;
            Type = type;
        }

        public string Id { get; set; }
        public string UniqueByProvider { get; set; } // Google: Email, GitHub: UserName

        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public OAuthType Type { get; set; }
    }
}
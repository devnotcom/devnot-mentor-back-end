namespace DevnotMentor.Api.CustomEntities.Auth
{
    public class OAuthUser
    {
        public OAuthUser(string id, string identifierProperty, string fullName, string profilePictureUrl, OAuthType type)
        {
            Id = id;
            IdentifierProperty = identifierProperty;
            FullName = fullName;
            ProfilePictureUrl = profilePictureUrl;
            Type = type;
        }

        public string Id { get; set; }
        public string IdentifierProperty { get; set; } // Google: Email, GitHub: UserName

        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public OAuthType Type { get; set; }
    }
}
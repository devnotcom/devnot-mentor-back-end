namespace DevnotMentor.Api.CustomEntities.Auth
{
    public class OAuthUser
    {
        public string Id { get; set; }
        public string IdentifierProperty { get; set; } // Google: Email, GitHub: UserName

        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public OAuthType Type { get; set; }
    }
}
namespace DevnotMentor.Api.CustomEntities.OAuth
{
    public class OAuthUser
    {
        public OAuthUser(OAuthResponse oAuthResponse, OAuthType oAuthType)
        {
            this.Id = oAuthResponse.id;
            this.FullName = oAuthResponse.name;
            this.Email = oAuthResponse.email;

            switch (oAuthType)
            {
                case OAuthType.GitHub:
                    this.UserName = oAuthResponse.login;
                    this.ProfilePictureUrl = oAuthResponse.avatar_url;
                    break;
                case OAuthType.Google:
                    this.UserName = System.IO.Path.GetRandomFileName();
                    this.ProfilePictureUrl = oAuthResponse.picture;
                    break;
            }
            
            this.Type = oAuthType;
        }

        public string Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public OAuthType Type { get; set; }
    }
}
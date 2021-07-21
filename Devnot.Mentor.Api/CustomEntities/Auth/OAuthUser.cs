using System;

namespace DevnotMentor.Api.CustomEntities.OAuth
{
    public class OAuthUser
    {
        public OAuthUser(OAuthResponse response, OAuthType type)
        {
            Type = type;

            FullName = response.name;
            Email = response.email;
            CreatedAt = DateTime.Now;

            switch (Type)
            {
                case OAuthType.GitHub:
                    GoogleId = null;

                    GitHubId = response.id;
                    UserName = response.login;
                    ProfilePictureUrl = response.avatar_url;

                    if (!string.IsNullOrEmpty(Email))
                    {
                        EmailConfirmed = true;
                    }
                    else
                    {
                        Email = UserName;
                    }
                    break;
                case OAuthType.Google:
                    GitHubId = null;

                    GoogleId = response.id;
                    UserName = System.IO.Path.GetRandomFileName();
                    ProfilePictureUrl = response.picture;
                    EmailConfirmed = true;
                    break;
            }
        }

        public string UserName { get; set; }
        public string GitHubId { get; set; }
        public string GoogleId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public OAuthType Type { get; set; }
    }
}
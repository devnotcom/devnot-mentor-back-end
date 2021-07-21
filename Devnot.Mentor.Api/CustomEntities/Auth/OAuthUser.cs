using System;

namespace DevnotMentor.Api.CustomEntities.OAuth
{
    public class OAuthUser
    {
        public OAuthUser(OAuthResponse response, OAuthType type)
        {
            Id = response.id;
            FullName = response.name;
            Email = response.email;

            switch (type)
            {
                case OAuthType.GitHub:
                    UserName = response.login;
                    ProfilePictureUrl = response.avatar_url;
                    if (Email != null)
                    {
                        EmailConfirmed = true;
                    }
                    else
                    {
                        Email = UserName;
                    }
                    break;
                case OAuthType.Google:
                    UserName = System.IO.Path.GetRandomFileName();
                    ProfilePictureUrl = response.picture;
                    EmailConfirmed = true;
                    break;
            }
            
            Type = type;
            CreatedAt = DateTime.Now;
        }

        public string Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public OAuthType Type { get; set; }
    }
}
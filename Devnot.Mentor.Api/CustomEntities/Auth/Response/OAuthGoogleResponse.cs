using System;

namespace DevnotMentor.Api.CustomEntities.Auth.Response
{
    public class OAuthGoogleResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string picture { get; set; }

        public OAuthGoogleUser MapToOAuthGoogleUser()
        {
            return new OAuthGoogleUser()
            {
                Email = email,
                Id = id,
                FullName = name,
                ProfilePictureUrl = picture,
                EmailConfirmed = (!String.IsNullOrEmpty(email)),
                UserName = Guid.NewGuid().ToString().Split('-')[0]
            };
        }
    }
}

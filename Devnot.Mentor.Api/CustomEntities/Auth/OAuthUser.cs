using System;
using System.Threading.Tasks;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;

namespace DevnotMentor.Api.CustomEntities.Auth
{
    public abstract class OAuthUser
    {
        protected OAuthUser(OAuthType providerType)
        {
            OAuthProviderType = providerType;
        }

        /// <summary>
        /// Provider ID
        /// </summary>
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ProfilePictureUrl { get; set; }
        public OAuthType OAuthProviderType { get; private set; }

        /// <summary>
        /// Get User by Provider
        /// </summary>
        /// <returns></returns>
        public abstract Task<User> GetUserFromDatabaseAsync(IUserRepository repository);

        public void SetRandomUsername()
        {
            this.UserName = Guid.NewGuid().ToString().Split('-')[0];
        }
    }
}
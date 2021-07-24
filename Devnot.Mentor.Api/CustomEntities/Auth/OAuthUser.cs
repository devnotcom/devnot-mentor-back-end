using System;
using System.Threading.Tasks;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;

namespace DevnotMentor.Api.CustomEntities.OAuth
{
    public abstract class OAuthUser
    {
        protected OAuthUser(OAuthType providerType)
        {
            OAuthProviderType = providerType;
        }


        /// <summary>
        /// O an ki provider'a ait kullan�c� id de�eri.
        /// </summary>
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ProfilePictureUrl { get; set; }
        public OAuthType OAuthProviderType { get; private set; }

        /// <summary>
        /// Provider kullan�c�s�n�n bilgilerine g�re veri taban�ndan ilgili kullan�c�y� getirir.
        /// </summary>
        /// <returns></returns>
        public abstract Task<User> GetUserFromDatabase(IUserRepository repository);

        public void SetRandomUsername()
        {
            this.UserName = Guid.NewGuid().ToString().Split('-')[0];
        }
    }
}
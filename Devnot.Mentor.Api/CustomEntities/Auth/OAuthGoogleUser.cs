using System;
using System.Threading.Tasks;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;

namespace DevnotMentor.Api.CustomEntities.Auth
{
    public class OAuthGoogleUser : OAuthUser
    {
        public OAuthGoogleUser() : base(OAuthType.Google)
        {
        }

        public override async Task<User> GetUserFromDatabaseAsync(IUserRepository repository)
        {
            if (repository is null)
            {
                throw new NullReferenceException($"{repository.GetType().FullName} was null.");
            }

            return await repository.GetByGoogleIdAsync(base.Id);
        }
    }
}

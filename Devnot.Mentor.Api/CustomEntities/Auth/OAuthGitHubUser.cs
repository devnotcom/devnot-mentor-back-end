using System;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.OAuth;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;

namespace DevnotMentor.Api.CustomEntities.Auth
{
    public class OAuthGitHubUser : OAuthUser
    {
        public OAuthGitHubUser() : base(OAuthType.GitHub)
        {
        }

        public override async Task<User> GetUserFromDatabase(IUserRepository repository)
        {
            if (repository is null)
            {
                throw new NullReferenceException($"{repository.GetType().FullName} was null.");
            }

            return await repository.GetByGitHubIdAsync(base.Id);
        }
    }
}

using DevnotMentor.Api.Entities;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetById(int id);
        Task<User> GetByGitHubId(object identifier);
        Task<User> GetByGoogleId(object identifier);
        Task<User> GetByUserName(string userName);
        Task<User> GetByEmail(string email);
        Task<bool> IsExists(int id);
    }
}

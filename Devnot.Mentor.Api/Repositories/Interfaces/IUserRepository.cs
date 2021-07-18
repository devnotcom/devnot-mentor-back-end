using DevnotMentor.Api.Entities;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByGitHubIdAsync(object identifier);
        Task<User> GetByGoogleIdAsync(object identifier);
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUserNameAsync(string userName);
        Task<User> GetByEmailAsync(string email);
        Task<bool> IsExistsAsync(int id);
    }
}

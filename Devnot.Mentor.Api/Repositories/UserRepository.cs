using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(MentorDBContext context) : base(context)
        {
        }

        public async Task<User> GetById(int id)
        {
            return await DbContext.User.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await DbContext.User.Where(i => i.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            return await DbContext.User.AnyAsync(i => i.Id == id);
        }

        public async Task<User> GetByUserName(string userName)
        {
            return await DbContext.User.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<User> GetByUserNameOrEmailAsync(object identifier)
        {
            return await DbContext.User.Where(u => u.GitHubId == identifier.ToString() || u.Email == identifier.ToString()).FirstOrDefaultAsync();
        }
    }
}

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

        public async Task<User> GetByIdAsync(int id)
        {
            return await DbContext.User.Where(i => i.Id == id).FirstOrDefaultAsync();
        }
        public async Task<User> GetByIdIncludeMenteeMentorAsync(int id)
        {
            return await DbContext.User
            .Include(x=>x.Mentor).Include(x=>x.Mentee)
            .Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbContext.User.Where(i => i.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            return await DbContext.User.AnyAsync(i => i.Id == id);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await DbContext.User.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<User> GetByGitHubIdAsync(object identifier)
        {
            return await DbContext.User.Where(u => u.GitHubId == identifier.ToString()).FirstOrDefaultAsync();
        }
        
        public async Task<User> GetByGoogleIdAsync(object identifier)
        {
            return await DbContext.User.Where(u => u.GoogleId == identifier.ToString()).FirstOrDefaultAsync();
        }
    }
}

using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<User> GetAsync(string userName, string hashedPassword)
        {
            return await DbContext
                .User
                .Where(u => u.UserName == userName && u.Password == hashedPassword)
                .Include(user => user.Mentee)
                .Include(user => user.Mentor)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetAsync(int userId, string hashedPassword)
        {
            return await DbContext.User.Where(i => i.Id == userId && i.Password == hashedPassword).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbContext.User.Where(i => i.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetAsync(Guid securityKey)
        {
            return await DbContext.User.Where(i => i.SecurityKey == securityKey).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            return await DbContext.User.AnyAsync(i => i.Id == id);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await DbContext.User.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }
    }
}

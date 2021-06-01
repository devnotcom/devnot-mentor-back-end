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

        public async Task<User> GetById(int id)
        {
            return await DbContext.User.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> Get(string userName, string hashedPassword)
        {
            return await DbContext.User.Where(u => u.UserName == userName && u.Password == hashedPassword).FirstOrDefaultAsync();
        }

        public async Task<User> Get(int userId, string hashedPassword)
        {
            return await DbContext.User.Where(i => i.Id == userId && i.Password == hashedPassword).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await DbContext.User.Where(i => i.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> Get(Guid securityKey)
        {
            return await DbContext.User.Where(i => i.SecurityKey == securityKey).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            return await DbContext.User.AnyAsync(i => i.Id == id);
        }

        public async Task<User> GetByUserName(string userName)
        {
            return await DbContext.User.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }
    }
}

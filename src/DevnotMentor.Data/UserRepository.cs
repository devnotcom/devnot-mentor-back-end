using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(MentorDBContext context) : base(context)
        {
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await DbContext.Users.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetAsync(string userName, string hashedPassword)
        {
            return await DbContext
                .Users
                .Where(u => u.UserName == userName && u.Password == hashedPassword)
                .Include(user => user.Mentee)
                    .ThenInclude(mentee => mentee.MenteeTags)
                        .ThenInclude(menteeTag => menteeTag.Tag)
                .Include(user => user.Mentor)
                    .ThenInclude(mentor => mentor.MentorTags)
                        .ThenInclude(mentorTag => mentorTag.Tag)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetAsync(int userId, string hashedPassword)
        {
            return await DbContext.Users.Where(i => i.Id == userId && i.Password == hashedPassword).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbContext.Users.Where(i => i.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetAsync(Guid securityKey)
        {
            return await DbContext.Users.Where(i => i.SecurityKey == securityKey).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            return await DbContext.Users.AnyAsync(i => i.Id == id);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await DbContext.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }
    }
}

using DevnotMentor.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class UserRepository : Repository<User>
    {
        private MentorDBContext _context;

        public UserRepository(MentorDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetById(int id)
        {
            return await _context.User.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> Get(string userName, string hashedPassword)
        {
            return await context.User.Where(u => u.UserName == userName && u.Password == hashedPassword).FirstOrDefaultAsync();
        }

        public async Task<User> Get(int userId, string hashedPassword)
        {
            return await context.User.Where(i => i.Id == userId && i.Password == hashedPassword).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsById(int id)
        {
            return await _context.User.AnyAsync(i => i.Id == id);
        }
    }
}

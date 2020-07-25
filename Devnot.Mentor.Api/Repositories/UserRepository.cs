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
        public UserRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<User> GetUser(string userName, string hashedPassword)
        {
            return await context.User.Where(u => u.UserName == userName && u.Password == hashedPassword).FirstOrDefaultAsync();
        }
    }
}

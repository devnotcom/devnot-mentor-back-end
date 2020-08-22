using DevnotMentor.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MenteeRepository : Repository<Mentee>
    {

        public MenteeRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<int> GetIdByUserId(int userId)
        {
            return await context.Mentee.Where(i => i.UserId == userId).Select(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsByUserId(int userId)
        {
            return await context.Mentee.AnyAsync(i => i.UserId == userId);
        }
    }
}

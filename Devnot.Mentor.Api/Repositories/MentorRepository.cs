using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevnotMentor.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevnotMentor.Api.Repositories
{
    public class MentorRepository : Repository<Mentor>
    {
        public MentorRepository(MentorDBContext context) : base(context)
        {
        }

        public async Task<int> GetIdByUserId(int userId)
        {
            return await context.Mentor.Where(i => i.UserId == userId).Select(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsByUserId(int userId)
        {
            return await context.Mentor.AnyAsync(i => i.UserId == userId);
        }
    }
}

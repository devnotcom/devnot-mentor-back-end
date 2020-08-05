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
        private MentorDBContext _context;

        public MenteeRepository(MentorDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetIdByUserIdAsync(int userId)
        {
            return await _context.Mentee.Where(i => i.UserId == userId).Select(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> AnyByUserIdAsync(int userId)
        {
            return await _context.Mentee.AnyAsync(i => i.UserId == userId);
        }
    }
}

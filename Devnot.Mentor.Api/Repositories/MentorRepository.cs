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
        private MentorDBContext _context;

        public MentorRepository(MentorDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetIdByUserIdAsync(int userId)
        {
            return await _context.Mentor.Where(i => i.UserId == userId).Select(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> AnyByUserIdAsync(int userId)
        {
            return await _context.Mentor.AnyAsync(i => i.UserId == userId);
        }
    }
}

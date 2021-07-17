using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MenteeRepository : BaseRepository<Mentee>, IMenteeRepository
    {

        public MenteeRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<Mentee> GetByUserIdAsync(int userId)
        {
            return await DbContext.Mentee.Where(i => i.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Mentee> GetByUserNameAsync(string userName)
        {
            return await DbContext.Mentee.Include(x => x.User).Where(i => i.User.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<int> GetIdByUserIdAsync(int userId)
        {
            return await DbContext.Mentee.Where(i => i.UserId == userId).Select(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsByUserIdAsync(int userId)
        {
            return await DbContext.Mentee.AnyAsync(i => i.UserId == userId);
        }

        public async Task<IEnumerable<Mentor>> GetPairedMentorsByMenteeIdAsync(int menteeId)
        {
            return await DbContext.MentorMenteePairs.Where(x => x.MenteeId == menteeId)
                .Include(x => x.Mentor)
                .ThenInclude(x => x.User)
                .Select(x => x.Mentor)
                .ToListAsync();
        }
    }
}

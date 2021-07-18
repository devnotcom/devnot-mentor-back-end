using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MentorApplicationsRepository : BaseRepository<MentorApplications>, IMentorApplicationsRepository
    {
        public MentorApplicationsRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<bool> IsExistsByUserIdAsync(int mentorId, int menteeId)
        {
            return await DbContext
                .MentorApplications
                .Where(i => i.MentorId == mentorId && i.MenteeId == menteeId)
                .AnyAsync();
        }

        public async Task<MentorApplications> GetAsync(int mentorId, int menteeId)
        {
            return await DbContext
                .MentorApplications
                .Where(i => i.MentorId == mentorId && i.MenteeId == menteeId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MentorApplications>> GetByUserIdAsync(int userId)
        {
            return await DbContext.MentorApplications
                .Include(x => x.Mentee).ThenInclude(x => x.User)
                .Include(x => x.Mentor).ThenInclude(x => x.User)
                .Where(x => x.Mentor.UserId == userId || x.Mentee.UserId == userId)
                .ToListAsync();
        }
    }
}

using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MentorApplicationsRepository : BaseRepository<MentorApplications>, IMentorApplicationsRepository
    {
        public MentorApplicationsRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<bool> IsExistsByUserId(int mentorId, int menteeId)
        {
            return await DbContext
                .MentorApplications
                .Where(i => i.MentorId == mentorId && i.MenteeId == menteeId)
                .AnyAsync();
        }

        public async Task<MentorApplications> Get(int mentorId, int menteeId)
        {
            return await DbContext
                .MentorApplications
                .Where(i => i.MentorId == mentorId && i.MenteeId == menteeId)
                .FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<MentorApplications>> GetForMentees(Expression<Func<MentorApplications, bool>> predicate)
        {
            return await DbContext.MentorApplications.Where(predicate).Include(x => x.Mentor).ToListAsync();
        }

        public async Task<IEnumerable<MentorApplications>> GetForMentors(Expression<Func<MentorApplications, bool>> predicate)
        {
            return await DbContext.MentorApplications.Where(predicate).Include(x => x.Mentee).ToListAsync();
        }
    }
}

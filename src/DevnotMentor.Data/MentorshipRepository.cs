using DevnotMentor.Data.Entities;
using DevnotMentor.Common.Enums;
using DevnotMentor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Data
{
    public class MentorshipRepository : BaseRepository<Mentorship>, IMentorshipRepository
    {
        public MentorshipRepository(MentorDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Mentorship>> GetMentorshipsByUserIdAsync(int userId)
        {
            return await DbContext.Mentorships
                .Include(x => x.Mentee).ThenInclude(x => x.User)
                .Include(x => x.Mentor).ThenInclude(x => x.User)
                .Where(x => x.Mentee.UserId == userId || x.Mentor.UserId == userId)
                .ToListAsync();
        }

        public int GetCountForContinuingStatusByMenteeId(int menteeId)
        {
            return DbContext.Mentorships.Count(i => i.MenteeId == menteeId && i.State == (int)MentorshipStatus.Continues);
        }

        public int GetCountForContinuingStatusByMentorId(int mentorId)
        {
            return DbContext.Mentorships.Count(i => i.MentorId == mentorId && i.State == (int)MentorshipStatus.Continues);
        }

        public async Task<Mentorship> GetWhichIsNotFinishedYetByIdAsync(int mentorshipId)
        {
            return await DbContext.Mentorships
               .Include(x => x.Mentee)
               .Include(x => x.Mentor)
               .FirstOrDefaultAsync(x => x.Id == mentorshipId && x.State != (int)MentorshipStatus.Finished);
        }

        public async Task<Mentorship> GetWhichIsFinishedByIdAsync(int mentorshipId)
        {
            return await DbContext.Mentorships
               .Include(x => x.Mentee).ThenInclude(x=>x.User)
               .Include(x => x.Mentor).ThenInclude(x=>x.User)
               .FirstOrDefaultAsync(x => x.Id == mentorshipId && x.State == (int)MentorshipStatus.Finished);
        }
    }
}

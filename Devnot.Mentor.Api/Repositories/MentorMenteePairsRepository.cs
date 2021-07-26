using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MentorMenteePairsRepository : BaseRepository<MentorMenteePairs>, IMentorMenteePairsRepository
    {
        public MentorMenteePairsRepository(MentorDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MentorMenteePairs>> GetPairsByUserIdAsync(int userId)
        {
            return await DbContext.MentorMenteePairs
                .Include(x => x.Mentee).ThenInclude(x => x.User)
                .Include(x => x.Mentor).ThenInclude(x => x.User)
                .Where(x => x.Mentee.UserId == userId || x.Mentor.UserId == userId)
                .ToListAsync();
        }

        public int GetCountForStatusContinuesByMenteeId(int menteeId)
        {
            return DbContext.MentorMenteePairs.Count(i => i.MenteeId == menteeId && i.State == MentorMenteePairStatus.Continues.ToInt());
        }

        public int GetCountForStatusContinuesByMentorId(int mentorId)
        {
            return DbContext.MentorMenteePairs.Count(i => i.MentorId == mentorId && i.State == MentorMenteePairStatus.Continues.ToInt());
        }

        public async Task<MentorMenteePairs> GetForStatusNotFinishedByIdAndAsync(int pairsId)
        {
            return await DbContext.MentorMenteePairs
               .Include(x => x.Mentee)
               .Include(x => x.Mentor)
               .FirstOrDefaultAsync(x => x.Id == pairsId && x.State != MentorMenteePairStatus.Finished.ToInt());
        }

        public async Task<MentorMenteePairs> GetForStatusFinishedByIdAndAsync(int pairsId)
        {
            return await DbContext.MentorMenteePairs
               .Include(x => x.Mentee)
               .Include(x => x.Mentor)
               .FirstOrDefaultAsync(x => x.Id == pairsId && x.State == MentorMenteePairStatus.Finished.ToInt());
        }
    }
}

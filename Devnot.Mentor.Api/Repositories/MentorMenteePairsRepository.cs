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

        public async Task<IEnumerable<MentorMenteePairs>> GetListByUserIdAsync(int userId)
        {
            return await DbContext.MentorMenteePairs
                .Include(x => x.Mentee).ThenInclude(x => x.User)
                .Include(x => x.Mentor).ThenInclude(x => x.User)
                .Where(x => x.Mentee.UserId == userId || x.Mentor.UserId == userId)
                .ToListAsync();
        }

        public int GetCountForContinuesStatusByMenteeId(int menteeId)
        {
            return DbContext.MentorMenteePairs.Count(i => i.MenteeId == menteeId && i.State == MentorMenteePairStatus.Continues.ToInt());
        }

        public int GetCountForContinuesStatusByMentorId(int mentorId)
        {
            return DbContext.MentorMenteePairs.Count(i => i.MentorId == mentorId && i.State == MentorMenteePairStatus.Continues.ToInt());
        }

        public async Task<MentorMenteePairs> GetByIdIncludeMenteeMentorAsync(int pairsId)
        {
             return await DbContext.MentorMenteePairs
                .Include(x => x.Mentee)
                .Include(x => x.Mentor)
                .FirstOrDefaultAsync(x => x.Id == pairsId);
        }
    }
}

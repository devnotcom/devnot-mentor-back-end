using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Repositories.Interfaces;
using System.Linq;

namespace DevnotMentor.Api.Repositories
{
    public class MentorMenteePairsRepository : BaseRepository<MentorMenteePairs>, IMentorMenteePairsRepository
    {
        public MentorMenteePairsRepository(MentorDBContext context) : base(context)
        {
        }

        public int GetCountForContinuesStatusByMenteeId(int menteeId)
        {
            return DbContext.MentorMenteePairs.Count(i => i.MenteeId == menteeId && i.State == MentorMenteePairStatus.Continues.ToInt());
        }

        public int GetCountForContinuesStatusByMentorId(int mentorId)
        {
            return DbContext.MentorMenteePairs.Count(i => i.MentorId == mentorId && i.State == MentorMenteePairStatus.Continues.ToInt());
        }
    }
}

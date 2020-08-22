using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MentorMenteePairsRepository : Repository<MentorMenteePairs>
    {
        public MentorMenteePairsRepository(MentorDBContext context) : base(context)
        {
        }

        public int GetCountForContinuesStatusByMenteeUserId(int menteeUserId)
        {
            return context.MentorMenteePairs.Where(i => i.Mentee.UserId == menteeUserId && i.State == MentorMenteePairStatus.Continues.ToInt()).Count();
        }

        public int GetCountForContinuesStatusByMentorUserId(int mentorUserId)
        {
            return context.MentorMenteePairs.Where(i => i.Mentor.UserId == mentorUserId && i.State == MentorMenteePairStatus.Continues.ToInt()).Count();
        }
    }
}

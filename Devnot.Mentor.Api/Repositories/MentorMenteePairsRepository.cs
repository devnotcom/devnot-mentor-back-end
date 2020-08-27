using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Repositories.Interfaces;
using System;
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

        public int GetCountForContinuesStatusByMenteeUserId(int menteeUserId)
        {
            return DbContext.MentorMenteePairs.Where(i => i.Mentee.UserId == menteeUserId && i.State == MentorMenteePairStatus.Continues.ToInt()).Count();
        }

        public int GetCountForContinuesStatusByMentorUserId(int mentorUserId)
        {
            return DbContext.MentorMenteePairs.Where(i => i.Mentor.UserId == mentorUserId && i.State == MentorMenteePairStatus.Continues.ToInt()).Count();
        }
    }
}

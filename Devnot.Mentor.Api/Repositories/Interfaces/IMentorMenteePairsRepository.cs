using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorMenteePairsRepository : IRepository<MentorMenteePairs>
    {
        int GetCountForContinuesStatusByMenteeUserId(int menteeUserId);
        int GetCountForContinuesStatusByMentorUserId(int mentorUserId);
    }
}

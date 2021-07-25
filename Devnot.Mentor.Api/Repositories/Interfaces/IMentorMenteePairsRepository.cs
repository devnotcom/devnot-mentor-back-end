using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Entities;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorMenteePairsRepository : IRepository<MentorMenteePairs>
    {
        int GetCountForContinuesStatusByMenteeId(int menteeId);
        int GetCountForContinuesStatusByMentorId(int mentorId);

        Task<IEnumerable<MentorMenteePairs>> GetListByUserIdAsync(int userId);

        Task<MentorMenteePairs> GetByIdIncludeMenteeMentorAsync(int pairsId);
    }
}

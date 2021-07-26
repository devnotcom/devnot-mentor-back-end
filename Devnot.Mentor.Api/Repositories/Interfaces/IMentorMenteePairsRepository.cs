using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Entities;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorMenteePairsRepository : IRepository<MentorMenteePairs>
    {
        int GetCountForStatusContinuesByMenteeId(int menteeId);
        int GetCountForStatusContinuesByMentorId(int mentorId);

        Task<IEnumerable<MentorMenteePairs>> GetPairsByUserIdAsync(int userId);

        Task<MentorMenteePairs> GetForStatusNotFinishedByIdAndAsync(int pairsId);
        Task<MentorMenteePairs> GetForStatusFinishedByIdAndAsync(int pairsId);
    }
}

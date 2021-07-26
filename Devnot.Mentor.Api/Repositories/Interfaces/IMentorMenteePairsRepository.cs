using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.Entities;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorMenteePairsRepository : IRepository<MentorMenteePairs>
    {
        int GetCountByMenteeIdAndStatusContinues(int menteeId);
        int GetCountByMentorIdAndStatusContinues(int mentorId);

        Task<IEnumerable<MentorMenteePairs>> GetPairsByUserIdAsync(int userId);

        Task<MentorMenteePairs> GetByIdAndStatusNotFinishedAsync(int pairsId);
        Task<MentorMenteePairs> GetByIdAndStatusFinishedAsync(int pairsId);
    }
}

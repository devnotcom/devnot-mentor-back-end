using DevnotMentor.Api.Entities;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorMenteePairsRepository : IRepository<MentorMenteePairs>
    {
        int GetCountForContinuesStatusByMenteeId(int menteeId);
        int GetCountForContinuesStatusByMentorId(int mentorId);
    }
}

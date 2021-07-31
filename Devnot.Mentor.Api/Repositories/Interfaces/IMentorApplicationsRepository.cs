using DevnotMentor.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorApplicationsRepository : IRepository<MentorApplications>
    {
        Task<bool> IsExistsByUserIdAsync(int mentorId, int menteeId);
        Task<MentorApplications> GetAsyncByMentorIdAndMenteeId(int mentorId, int menteeId);
        Task<IEnumerable<MentorApplications>> GetApplicationsByUserIdAsync(int userId);
    }
}

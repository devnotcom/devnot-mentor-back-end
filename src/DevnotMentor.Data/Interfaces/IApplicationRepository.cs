using DevnotMentor.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevnotMentor.Data.Interfaces
{
    public interface IApplicationRepository : IRepository<Application>
    {
        Task<bool> AnyWaitingApplicationBetweenMentorAndMenteeAsync(int mentorId, int menteeId);
        Task<Application> GetWhichIsWaitingByIdAsync(int applicationId);
        Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(int userId);
    }
}

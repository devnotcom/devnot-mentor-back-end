using DevnotMentor.Api.Entities;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorApplicationsRepository : IRepository<MentorApplications>
    {
        Task<bool> IsExistsByUserId(int mentorId, int menteeId);
        Task<MentorApplications> Get(int mentorId, int menteeId);
    }
}

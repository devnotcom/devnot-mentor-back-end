using DevnotMentor.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorRepository : IRepository<Mentor>
    {
        Task<int> GetIdByUserIdAsync(int userId);
        Task<Mentor> GetByUserNameAsync(string userName);
        Task<Mentor> GetByUserIdAsync(int userId);
        Task<bool> IsExistsByUserIdAsync(int userId);
        Task<IEnumerable<Mentee>> GetPairedMenteesByMentorIdAsync(int mentorId);
    }
}

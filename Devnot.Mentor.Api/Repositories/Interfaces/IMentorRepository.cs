using DevnotMentor.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorRepository : IRepository<Mentor>
    {
        Task<int> GetIdByUserId(int userId);
        Task<Mentor> GetByUserName(string userName);
        Task<Mentor> GetByUserId(int userId);
        Task<bool> IsExistsByUserId(int userId);
        Task<IEnumerable<Mentee>> GetPairedMenteesByMentorId(int mentorId);
    }
}

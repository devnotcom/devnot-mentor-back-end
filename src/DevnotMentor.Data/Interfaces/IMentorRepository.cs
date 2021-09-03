using DevnotMentor.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.Requests;

namespace DevnotMentor.Data.Interfaces
{
    public interface IMentorRepository : IRepository<Mentor>
    {
        Task<List<Mentor>> SearchAsync(SearchRequest request);
        Task<Mentor> GetByIdAsync(int mentorId);
        Task<int> GetIdByUserIdAsync(int userId);
        Task<Mentor> GetByUserNameAsync(string userName);
        Task<Mentor> GetByUserIdAsync(int userId);
        Task<bool> IsExistsByUserIdAsync(int userId);
        Task<IEnumerable<Mentee>> GetPairedMenteesByMentorIdAsync(int mentorId);
    }
}

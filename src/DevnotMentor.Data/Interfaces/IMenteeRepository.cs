using DevnotMentor.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Common.Requests;

namespace DevnotMentor.Data.Interfaces
{
    public interface IMenteeRepository : IRepository<Mentee>
    {
        Task<List<Mentee>> SearchAsync(SearchRequest request);
        Task<Mentee> GetByIdAsync(int mentorId);
        Task<Mentee> GetByUserIdAsync(int userId);
        Task<Mentee> GetByUserNameAsync(string userName);
        Task<int> GetIdByUserIdAsync(int userId);
        Task<bool> IsExistsByUserIdAsync(int userId);
        Task<IEnumerable<Mentor>> GetPairedMentorsByMenteeIdAsync(int menteeId);
    }
}

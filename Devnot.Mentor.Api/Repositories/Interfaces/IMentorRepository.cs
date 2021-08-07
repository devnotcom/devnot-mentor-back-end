using DevnotMentor.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Request.CommonRequest;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorRepository : IRepository<Mentor>
    {
        Task<List<Mentor>> SearchAsync(SearchRequest request);
        Task<int> GetIdByUserIdAsync(int userId);
        Task<Mentor> GetByUserNameAsync(string userName);
        Task<Mentor> GetByUserIdAsync(int userId);
        Task<bool> IsExistsByUserIdAsync(int userId);
        Task<IEnumerable<Mentee>> GetPairedMenteesByMentorIdAsync(int mentorId);
    }
}

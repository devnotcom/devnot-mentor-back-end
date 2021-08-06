using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Request.CommonRequest;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMenteeRepository : IRepository<Mentee>
    {
        Task<List<Mentee>> SearchAsync(SearchRequest request);
        Task<Mentee> GetByUserIdAsync(int userId);
        Task<Mentee> GetByUserNameAsync(string userName);
        Task<int> GetIdByUserIdAsync(int userId);
        Task<bool> IsExistsByUserIdAsync(int userId);
        Task<IEnumerable<Mentor>> GetPairedMentorsByMenteeIdAsync(int menteeId);
    }
}

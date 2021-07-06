using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMenteeRepository : IRepository<Mentee>
    {
        Task<Mentee> GetByUserId(int userId);
        Task<Mentee> GetByUserName(string userName);
        Task<int> GetIdByUserId(int userId);
        Task<bool> IsExistsByUserId(int userId);

        Task<IEnumerable<Mentor>> GetPairedMentorsByMenteeId(int menteeId);

    }
}

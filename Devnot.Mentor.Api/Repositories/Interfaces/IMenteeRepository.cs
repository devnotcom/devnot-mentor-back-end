using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMenteeRepository : IRepository<Mentee>
    {
        Task<Mentee> GetByUserId(int userId);
        Task<int> GetIdByUserId(int userId);
        Task<bool> IsExistsByUserId(int userId);
    }
}

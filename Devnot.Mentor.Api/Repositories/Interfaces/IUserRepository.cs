using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetById(int id);
        Task<User> GetByUserName(string userName);
        Task<User> Get(string userName, string hashedPassword);
        Task<User> Get(int userId, string hashedPassword);
        Task<User> GetByEmail(string email);
        Task<User> Get(Guid securityKey);
        Task<bool> IsExists(int id);
    }
}

using DevnotMentor.Data.Entities;
using System;
using System.Threading.Tasks;

namespace DevnotMentor.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUserNameAsync(string userName);
        Task<User> GetAsync(string userName, string hashedPassword);
        Task<User> GetAsync(int userId, string hashedPassword);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetAsync(Guid securityKey);
        Task<bool> IsExistsAsync(int id);
    }
}

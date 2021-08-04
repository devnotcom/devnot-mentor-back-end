using DevnotMentor.Api.Entities;
using System;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface ILoggerRepository : IRepository<Log>
    {
        Task WriteInfoAsync(string message);
        Task WriteErrorAsync(string message);
        Task WriteErrorAsync(Exception ex);
        Task WriteLogAsync(string message, string stackTrace, string level);
    }
}

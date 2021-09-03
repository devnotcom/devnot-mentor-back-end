using DevnotMentor.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevnotMentor.Data.Interfaces
{
    public interface ILogRepository : IRepository<Log>
    {
        Task<List<Log>> GetListAsync(int count);
        Task WriteInfoAsync(string message);
        Task WriteErrorAsync(string message);
        Task WriteErrorAsync(Exception ex);
        Task WriteLogAsync(string message, string stackTrace, string level);
    }
}

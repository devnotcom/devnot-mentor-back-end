using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface ILoggerRepository : IRepository<Log>
    {
        Task WriteInfo(string message);
        Task WriteError(string message);
        Task WriteError(Exception ex);
        Task WriteLog(string message, string stackTrace, string level);
    }
}

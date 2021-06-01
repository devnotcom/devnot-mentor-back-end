using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class LoggerRepository : BaseRepository<Log>, ILoggerRepository
    {
        public LoggerRepository(MentorDBContext context) : base(context)
        {
        }

        public async Task WriteInfo(string message)
        {
            await WriteLog(message, null, "INFO");
        }

        public async Task WriteError(string message)
        {
            await WriteLog(message, null, "ERROR");
        }

        public async Task WriteError(Exception ex)
        {
            var message = ex.Message;
            var stackTrace = ex.StackTrace;

            if (ex.InnerException != null)
            {
                message += Environment.NewLine + "InnerException: " + ex.InnerException.Message;
                stackTrace += Environment.NewLine + "InnerException: " + ex.InnerException.StackTrace;
            }
            await WriteLog(message, stackTrace, "ERROR");
        }

        public async Task WriteLog(string message, string stackTrace, string level)
        {
            var log = new Log()
            {
                Id = Guid.NewGuid(),
                Level = level,
                Message = message,
                StackTrace = stackTrace,
                InsertDate = DateTime.Now
            };

            DbContext.Log.Add(log);
            await DbContext.SaveChangesAsync();
        }
    }
}

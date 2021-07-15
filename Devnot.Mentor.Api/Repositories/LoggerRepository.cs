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

        public async Task WriteInfoAsync(string message)
        {
            await WriteLogAsync(message, null, "INFO");
        }

        public async Task WriteErrorAsync(string message)
        {
            await WriteLogAsync(message, null, "ERROR");
        }

        public async Task WriteErrorAsync(Exception ex)
        {
            var message = ex.Message;
            var stackTrace = ex.StackTrace;

            if (ex.InnerException != null)
            {
                message += Environment.NewLine + "InnerException: " + ex.InnerException.Message;
                stackTrace += Environment.NewLine + "InnerException: " + ex.InnerException.StackTrace;
            }
            await WriteLogAsync(message, stackTrace, "ERROR");
        }

        public async Task WriteLogAsync(string message, string stackTrace, string level)
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

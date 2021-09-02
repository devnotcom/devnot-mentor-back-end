using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DevnotMentor.Data
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        public LogRepository(MentorDBContext context) : base(context)
        {
        }

        public Task<List<Log>> GetListAsync(int count)
        {
            return DbContext.Logs.OrderByDescending(log => log.InsertedAt).Take(count).ToListAsync();
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
                InsertedAt = DateTime.Now
            };

            DbContext.Logs.Add(log);
            await DbContext.SaveChangesAsync();
        }
    }
}

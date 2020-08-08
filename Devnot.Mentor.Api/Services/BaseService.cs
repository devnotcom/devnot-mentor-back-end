using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Services
{
    public class BaseService
    {
        protected AppSettings appSettings;
        protected ResponseMessages responseMessages;
        protected IMapper mapper;
        protected MentorDBContext context;
        protected LoggerRepository logger;

        public BaseService(IOptions<AppSettings> appSettings, IOptions<ResponseMessages> responseMessages, IMapper mapper, MentorDBContext context)
        {
            this.appSettings = appSettings.Value;
            this.mapper = mapper;
            this.context = context;
            this.responseMessages = responseMessages.Value;
            logger = new LoggerRepository(this.context);
        }

        public async Task RunInTry(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                await logger.WriteError(ex);
            }
        }

        public async Task RunInTry<T>(ApiResponse<T> response, Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                response.Message = responseMessages.Values["UnhandledException"];
                await logger.WriteError(ex);
            }
        }

        public async Task RunInTry(Func<Task> action, Func<Exception, Task> exception)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                await logger.WriteError(ex);
                await exception(ex);
            }
        }
    }
}

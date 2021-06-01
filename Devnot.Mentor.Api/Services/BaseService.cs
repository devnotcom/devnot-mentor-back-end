using AutoMapper;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Repositories.Interfaces;
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
        protected ILoggerRepository logger;

        public BaseService(
            IOptions<AppSettings> appSettings,
            IOptions<ResponseMessages> responseMessages,
            IMapper mapper,
            ILoggerRepository logger)
        {
            this.appSettings = appSettings.Value;
            this.mapper = mapper;
            this.responseMessages = responseMessages.Value;
            this.logger = logger;
        }

    }
}

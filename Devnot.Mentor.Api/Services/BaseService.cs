using AutoMapper;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Repositories;
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

    }
}

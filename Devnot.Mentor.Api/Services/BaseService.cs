using AutoMapper;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.Extensions.Options;

namespace DevnotMentor.Api.Services
{
    public class BaseService
    {
        protected AppSettings appSettings;
        protected IMapper mapper;
        protected ILoggerRepository logger;

        public BaseService(
            IOptions<AppSettings> appSettings,
            IMapper mapper,
            ILoggerRepository logger)
        {
            this.appSettings = appSettings.Value;
            this.mapper = mapper;
            this.logger = logger;
        }
    }
}

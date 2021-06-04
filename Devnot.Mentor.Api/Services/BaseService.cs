using AutoMapper;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.Repositories.Interfaces;

namespace DevnotMentor.Api.Services
{
    public class BaseService
    {
        protected IMapper mapper;
        protected ILoggerRepository logger;
        protected IDevnotConfigurationContext devnotConfigurationContext;
        public BaseService(
            IMapper mapper,
            ILoggerRepository logger, IDevnotConfigurationContext devnotConfigurationContext)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.devnotConfigurationContext = devnotConfigurationContext;
        }
    }
}

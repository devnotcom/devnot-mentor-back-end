using AutoMapper;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Data.Interfaces;

namespace DevnotMentor.Services
{
    public class BaseService
    {
        protected IMapper mapper;
        protected ILogRepository logger;
        protected IDevnotConfigurationContext devnotConfigurationContext;
        public BaseService(
            IMapper mapper,
            ILogRepository logger, IDevnotConfigurationContext devnotConfigurationContext)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.devnotConfigurationContext = devnotConfigurationContext;
        }
    }
}

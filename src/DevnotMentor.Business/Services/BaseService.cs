using AutoMapper;
using DevnotMentor.Configuration.Context;
using DevnotMentor.Data.Interfaces;

namespace DevnotMentor.Business.Services
{
    public class BaseService
    {
        protected IMapper _mapper;
        protected ILogRepository _log;
        protected IDevnotConfigurationContext _devnotConfigurationContext;
        
        public BaseService(
            IMapper mapper,
            ILogRepository logger, 
            IDevnotConfigurationContext devnotConfigurationContext)
        {
            this._mapper = mapper;
            this._log = logger;
            this._devnotConfigurationContext = devnotConfigurationContext;
        }
    }
}

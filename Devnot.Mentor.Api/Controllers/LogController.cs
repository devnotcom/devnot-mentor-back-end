using DevnotMentor.Api.Configuration.Environment;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    [ApiController]
    [Route("logs")]
    public class LogController : ControllerBase
    {
        private readonly ILoggerRepository loggerRepository;
        private readonly IEnvironmentService environmentService;
        
        public LogController(ILoggerRepository loggerRepository, IEnvironmentService environmentService)
        {
            this.loggerRepository = loggerRepository;
            this.environmentService = environmentService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (environmentService.IsDevelopment)
            {
                var logs = loggerRepository.GetList();
                return Ok(logs);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Create()
        {
            if (environmentService.IsDevelopment)
            {
                throw new System.Exception();
            }

            return NotFound();
        }
    }
}
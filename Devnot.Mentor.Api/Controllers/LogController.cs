using System.Threading.Tasks;
using DevnotMentor.Api.Configuration.Environment;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    [ApiController]
    [Route("logs")]
    public class LogController : ControllerBase
    {
        private readonly ILoggerRepository _loggerRepository;
        private readonly IEnvironmentService _environmentService;

        public LogController(ILoggerRepository loggerRepository, IEnvironmentService environmentService)
        {
            _loggerRepository = loggerRepository;
            _environmentService = environmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int count = 5)
        {
            if (_environmentService.IsDevelopment)
            {
                var logs = await _loggerRepository.GetListAsync(count);
                return Ok(logs);
            }

            return NotFound();
        }

        /// <summary>
        /// It creates new dummy log for development environment.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create()
        {
            if (_environmentService.IsDevelopment)
            {
                throw new System.Exception();
            }

            return NotFound();
        }
    }
}
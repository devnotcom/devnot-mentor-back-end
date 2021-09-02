using System.Threading.Tasks;
using DevnotMentor.Configurations.Environment;
using DevnotMentor.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.WebAPI.Controllers
{
    [ApiController]
    [Route("logs")]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository _logRepository;
        private readonly IEnvironmentService _environmentService;

        public LogController(ILogRepository loggerRepository, IEnvironmentService environmentService)
        {
            _logRepository = loggerRepository;
            _environmentService = environmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int count = 5)
        {
            if (_environmentService.IsDevelopment)
            {
                var logs = await _logRepository.GetListAsync(count);
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
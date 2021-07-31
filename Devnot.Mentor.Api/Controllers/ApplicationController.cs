using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    [ApiController]
    [Route("applications")]
    public class ApplicationController : BaseController
    {
        private readonly IApplicationService applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }

        [HttpGet("me")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetApplications()
        {
            var result = await applicationService.GetApplicationsByUserIdAsync(User.Claims.GetUserId());

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}
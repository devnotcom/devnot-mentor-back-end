using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    [ApiController]
    public class ApplicationController : BaseController
    {
        private readonly IApplicationService _applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("/users/me/applications")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetApplicationsAsync()
        {
            var result = await _applicationService.GetApplicationsByUserIdAsync(User.GetId());
            return ApiResponse(result);
        }
    }
}
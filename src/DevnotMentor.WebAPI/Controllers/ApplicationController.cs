using System.Threading.Tasks;
using DevnotMentor.WebAPI.ActionFilters;
using DevnotMentor.WebAPI.Helpers.Extensions;
using DevnotMentor.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.WebAPI.Controllers
{
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
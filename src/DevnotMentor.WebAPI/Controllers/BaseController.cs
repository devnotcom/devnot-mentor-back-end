using Microsoft.AspNetCore.Mvc;
using DevnotMentor.Common.API;

namespace DevnotMentor.WebAPI.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        protected IActionResult ApiResponse(ApiResponse response) => StatusCode(response.StatusCode, response);

        [NonAction]
        protected IActionResult ApiResponse<T>(ApiResponse<T> response) => StatusCode(response.StatusCode, response);
    }
}

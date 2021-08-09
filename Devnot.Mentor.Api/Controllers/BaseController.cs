using Microsoft.AspNetCore.Mvc;
using DevnotMentor.Api.Common.Response;

namespace DevnotMentor.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        protected IActionResult Response(ApiResponse response)
        {
            return StatusCode((int)response.ResponseStatus, response);
        }

        [NonAction]
        protected IActionResult Response<T>(ApiResponse<T> response)
        {
            return StatusCode((int)response.ResponseStatus, response);
        }
    }
}

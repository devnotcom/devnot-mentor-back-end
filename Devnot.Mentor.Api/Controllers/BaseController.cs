using Microsoft.AspNetCore.Mvc;
using DevnotMentor.Api.Common.Response;

namespace DevnotMentor.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        protected IActionResult Success(ApiResponse response)
        {
            return Ok(response);
        }

        [NonAction]
        protected IActionResult Success<T>(ApiResponse<T> response)
        {
            return Ok(response);
        }

        [NonAction]
        protected IActionResult Created(ApiResponse response)
        {
            return StatusCode(201, response);
        }

        [NonAction]
        protected IActionResult Created<T>(ApiResponse<T> response)
        {
            return StatusCode(201, response);
        }

        [NonAction]
        protected IActionResult NoContent(ApiResponse response)
        {
            return StatusCode(204, response);
        }

        [NonAction]
        protected IActionResult NoContent<T>(ApiResponse<T> response)
        {
            return StatusCode(204, response);
        }

        [NonAction]
        protected IActionResult BadRequest(ApiResponse response)
        {
            return StatusCode(400, response);
        }

        [NonAction]
        protected IActionResult BadRequest<T>(ApiResponse<T> response)
        {
            return StatusCode(400, response);
        }

        [NonAction]
        protected IActionResult Unauthorized(ApiResponse response)
        {
            return StatusCode(401, response);
        }

        [NonAction]
        protected IActionResult Unauthorized<T>(ApiResponse<T> response)
        {
            return StatusCode(401, response);
        }

        [NonAction]
        protected IActionResult Forbidden(ApiResponse response)
        {
            return StatusCode(403, response);
        }

        [NonAction]
        protected IActionResult Forbidden<T>(ApiResponse<T> response)
        {
            return StatusCode(403, response);
        }

        [NonAction]
        protected IActionResult NotFound(ApiResponse response)
        {
            return StatusCode(404, response);
        }

        [NonAction]
        protected IActionResult NotFound<T>(ApiResponse<T> response)
        {
            return StatusCode(404, response);
        }

        [NonAction]
        protected IActionResult Error(ApiResponse response)
        {
            return StatusCode(500, response);
        }

        [NonAction]
        protected IActionResult Error<T>(ApiResponse<T> response)
        {
            return StatusCode(500, response);
        }
    }
}

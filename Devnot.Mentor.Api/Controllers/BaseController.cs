using DevnotMentor.Api.Common;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }

		[NonAction]
		protected IActionResult Success<T>(T data)
		{
			return Success(new ApiResponse<T>
			{
				Success = true,
				Data = data
			});
		}

		[NonAction]
		protected IActionResult Success<T>(ApiResponse<T> response)
		{
			return Ok(response);
		}

		[NonAction]
		protected IActionResult Created<T>(string message, T data)
		{
			return Created(new ApiResponse<T>
			{
				Success = true,
				Message = message,
				Data = data
			});
		}

		[NonAction]
		protected IActionResult Created<T>(ApiResponse<T> data)
		{
			return StatusCode(201, data);
		}

		[NonAction]
		protected IActionResult NoContent(string message)
		{
			return StatusCode(204, new ApiResponse
			{
				Success = true,
				Message = message,
			});
		}

		[NonAction]
		protected IActionResult NoContent<T>(string message, T data)
		{
			return StatusCode(204, new ApiResponse<T>
			{
				Success = true,
				Message = message,
				Data = data
			});
		}

		[NonAction]
		protected IActionResult BadRequest<T>(string message, T data)
		{
			return BadRequest(new ApiResponse<T>
			{
				Success = false,
				Message = message,
				Data = data
			});
		}

		[NonAction]
		protected IActionResult BadRequest<T>(ApiResponse<T> data)
		{
			return StatusCode(400, data);
		}

		[NonAction]
		protected IActionResult Unauthorized<T>(string message, T data)
		{
			return Unauthorized(new ApiResponse<T>
			{
				Success = false,
				Message = message,
				Data = data
			});
		}

		[NonAction]
		protected IActionResult Unauthorized<T>(ApiResponse<T> data)
		{
			return StatusCode(401, data);
		}

		[NonAction]
		protected IActionResult Forbidden<T>(string message, T data)
		{
			return Forbidden(new ApiResponse<T>
			{
				Success = false,
				Message = message,
				Data = data
			});
		}

		[NonAction]
		protected IActionResult Forbidden<T>(ApiResponse<T> data)
		{
			return StatusCode(403, data);
		}

		[NonAction]
		protected IActionResult NotFound<T>(string message)
		{
			return StatusCode(404, new ApiResponse
			{
				Success = false,
				Message = message,
			});
		}

		[NonAction]
		protected IActionResult Error<T>(string message, T data)
		{
			return Error(new ApiResponse<T>
			{
				Success = false,
				Message = message,
				Data = data
			});
		}

		[NonAction]
		protected IActionResult Error<T>(ApiResponse<T> data)
		{
			return StatusCode(500, data);
		}
	}
}

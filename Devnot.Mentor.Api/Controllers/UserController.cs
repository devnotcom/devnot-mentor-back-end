using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.CustomEntities.Request.UserRequest;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("/users/login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var result = await userService.LoginAsync(request);

            return ApiResponse(result);
        }

        [HttpPost]
        [Route("/users/register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserRequest request)
        {
            var result = await userService.RegisterAsync(request);

            return ApiResponse(result);
        }

        [HttpPost]
        [Route("/users/change-password")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequest request)
        {
            request.UserId = User.Claims.GetUserId();

            var result = await userService.ChangePasswordAsync(request);

            return ApiResponse(result);
        }

        [HttpPatch]
        [Route("/users")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserRequest request)
        {
            request.UserId = User.Claims.GetUserId();

            var result = await userService.UpdateAsync(request);

            return ApiResponse(result);
        }

        [Route("/users/{email}/remind-password")]
        [HttpGet]
        public async Task<IActionResult> RemindPassword([FromRoute] string email)
        {
            var result = await userService.RemindPasswordAsync(email);

            return ApiResponse(result);
        }

        [HttpPost]
        [Route("/users/me/remind-password-complete")]
        public async Task<IActionResult> RemindPasswordCompleteAsync(CompleteRemindPasswordRequest request)
        {
            var result = await userService.RemindPasswordCompleteAsync(request);

            return ApiResponse(result);
        }
    }
}
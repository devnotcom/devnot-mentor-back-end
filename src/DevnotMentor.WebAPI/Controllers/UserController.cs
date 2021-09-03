using System.Threading.Tasks;
using DevnotMentor.WebAPI.ActionFilters;
using DevnotMentor.Common.Requests.User;
using DevnotMentor.WebAPI.Helpers.Extensions;
using DevnotMentor.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.WebAPI.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("/users/login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var result = await _userService.LoginAsync(request);

            return ApiResponse(result);
        }

        [HttpPost]
        [Route("/users/register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserRequest request)
        {
            var result = await _userService.RegisterAsync(request);

            return ApiResponse(result);
        }

        [HttpPost]
        [Route("/users/change-password")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequest request)
        {
            request.UserId = User.GetId();

            var result = await _userService.ChangePasswordAsync(request);

            return ApiResponse(result);
        }

        [HttpPatch]
        [Route("/users")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserRequest request)
        {
            request.UserId = User.GetId();

            var result = await _userService.UpdateAsync(request);

            return ApiResponse(result);
        }

        [Route("/users/{email}/remind-password")]
        [HttpGet]
        public async Task<IActionResult> RemindPassword([FromRoute] string email)
        {
            var result = await _userService.RemindPasswordAsync(email);

            return ApiResponse(result);
        }

        [HttpPost]
        [Route("/users/me/remind-password-complete")]
        public async Task<IActionResult> RemindPasswordCompleteAsync(CompleteRemindPasswordRequest request)
        {
            var result = await _userService.RemindPasswordCompleteAsync(request);

            return ApiResponse(result);
        }
    }
}
using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    [ServiceFilter(typeof(TokenAuthentication)), ApiController, Route("users"),]
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await userService.GetByUserIdAsync(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}
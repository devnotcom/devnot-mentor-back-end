using System.Security.Claims;
using System.Threading.Tasks;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route("/auth-github")]
        [HttpGet]
        public IActionResult GitHubChallenge()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/auth-github-logics" }, "GitHub");
        }

        [Route("/auth-github-logics")]
        [HttpGet]
        public async Task<IActionResult> GitHubLogics()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var id = claimsIdentity.FindFirst("github:id").Value;
            var name = claimsIdentity.FindFirst("github:name").Value;
            var login = claimsIdentity.FindFirst("github:login").Value;
            var avatar = claimsIdentity.FindFirst("github:avatar").Value;
            var result = await userService.GitHubAuth(id, name, login, avatar);
            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}
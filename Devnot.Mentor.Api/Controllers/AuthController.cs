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

        [Route("/auth/github")]
        [HttpGet]
        public IActionResult GitHubChallenge()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/" }, "GitHub");
        }
    }
}
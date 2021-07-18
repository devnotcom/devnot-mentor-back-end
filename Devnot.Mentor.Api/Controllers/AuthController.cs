using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    public class AuthController : BaseController
    {
        [Route("/auth/github")]
        [HttpGet]
        public IActionResult GitHubChallenge()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/" }, "GitHub");
        }
        [Route("/auth/google")]
        [HttpGet]
        public IActionResult GoogleChallenge()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/" }, "Google");
        }
    }
}
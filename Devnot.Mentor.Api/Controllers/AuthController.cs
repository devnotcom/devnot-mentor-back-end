using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    public class AuthController : BaseController
    {
        [Route("/auth/okay")]
        public IActionResult OK()
        {
            return Ok("OAuth: okay");
        }

        [Route("/auth/github")]
        [HttpGet]
        public IActionResult GitHubChallenge()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/auth/okay" }, "GitHub");
        }
        [Route("/auth/google")]
        [HttpGet]
        public IActionResult GoogleChallenge()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/auth/okay" }, "Google");
        }
    }
}
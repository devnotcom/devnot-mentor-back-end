using System.Threading.Tasks;
using DevnotMentor.Api.Services.Interfaces;
using DevnotMentor.Api.Utilities.OAuth;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("/auth/github")]
        [HttpPost]
        public async Task<IActionResult> GitHubAsync([FromBody] string accesToken)
        {
            var oAuthGitHubUser = await OAuthService.GetOAuthGitHubUserAsync(accesToken);
            var githubSignInResponse = await _userService.SignInAsync(oAuthGitHubUser);

            return githubSignInResponse.Success ? Success(githubSignInResponse) : BadRequest();
        }

        [Route("/auth/google")]
        [HttpPost]
        public async Task<IActionResult> GoogleAsync([FromBody] string accesToken)
        {
            var oAuthGoogleUser = await OAuthService.GetOAuthGoogleUserAsync(accesToken);
            var googleSignInResponse = await _userService.SignInAsync(oAuthGoogleUser);

            return googleSignInResponse.Success ? Success(googleSignInResponse) : BadRequest();
        }
    }
}
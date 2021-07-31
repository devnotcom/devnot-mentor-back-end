using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;
using DevnotMentor.Api.Helpers.Extensions;

namespace DevnotMentor.Api.Controllers
{
    [ValidateModelState]
    [ApiController]
    [Route("/mentees/")]
    public class MenteeController : BaseController
    {
        private readonly IMenteeService menteeService;
        private readonly IPairService pairService;
        private readonly IApplicationService applicationService;

        public MenteeController(IMenteeService menteeService, IPairService pairService, IApplicationService applicationService)
        {
            this.menteeService = menteeService;
            this.pairService = pairService;
            this.applicationService = applicationService;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetMenteeProfileAsync([FromRoute] string userName)
        {
            var result = await menteeService.GetMenteeProfileAsync(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/paireds/mentors")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetPairedMentorsAsync()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await menteeService.GetPairedMentorsByUserIdAsync(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/paireds")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetMentorshipsAsync()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await pairService.GetMentorshipsOfMenteeByUserId(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> CreateMenteeProfileAsync([FromBody] CreateMenteeProfileRequest request)
        {
            request.UserId = User.Claims.GetUserId();

            var result = await menteeService.CreateMenteeProfileAsync(request);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost("me/applications")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> CreateApplicationAsync(ApplicationRequest request)
        {
            request.MenteeUserId = User.Claims.GetUserId();

            var result = await applicationService.CreateApplicationAsync(request);

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}

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

        public MenteeController(IMenteeService menteeService, IPairService pairService)
        {
            this.menteeService = menteeService;
            this.pairService = pairService;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> Get([FromRoute] string userName)
        {
            var result = await menteeService.GetMenteeProfileAsync(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/paireds/mentors")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetPairedMentors()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await menteeService.GetPairedMentorsByUserIdAsync(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/paireds")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetMentorships()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await pairService.GetMentorshipsOfMenteeByUserId(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/applications")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetApplications()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await menteeService.GetApplicationsByUserIdAsync(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> Post([FromBody] CreateMenteeProfileRequest request)
        {
            request.UserId = User.Claims.GetUserId();

            var result = await menteeService.CreateMenteeProfileAsync(request);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost("me/applications")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> ApplyToMentor(ApplyToMentorRequest request)
        {
            request.MenteeUserId = User.Claims.GetUserId();

            var result = await menteeService.ApplyToMentorAsync(request);

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}

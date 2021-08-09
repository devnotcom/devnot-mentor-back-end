using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Request.CommonRequest;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;
using DevnotMentor.Api.Helpers.Extensions;

namespace DevnotMentor.Api.Controllers
{
    [ValidateModelState]
    [ApiController]
    [Route("/mentors/")]
    public class MentorController : BaseController
    {
        private readonly IMentorService mentorService;
        private readonly IPairService pairService;

        public MentorController(IMentorService mentorService, IPairService pairService)
        {
            this.mentorService = mentorService;
            this.pairService = pairService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            var result = await mentorService.SearchAsync(request);

            return ApiResponse(result);
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetAsync([FromRoute] string userName)
        {
            var result = await mentorService.GetMentorProfileAsync(userName);

            return ApiResponse(result);
        }

        [HttpGet("me/paireds/mentees")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetPairedMentees()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await mentorService.GetPairedMenteesByUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpGet("me/paireds")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetMentorships()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await pairService.GetMentorshipsOfMentorByUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpGet("me/applications")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetApplications()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await mentorService.GetApplicationsByUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> Post([FromBody] CreateMentorProfileRequest request)
        {
            request.UserId = User.Claims.GetUserId();

            var result = await mentorService.CreateMentorProfileAsync(request);

            return ApiResponse(result);
        }

        [HttpPost("{mentorId}/applications/mentees/{menteeId}/accept")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> AcceptMentee([FromRoute] int mentorId, [FromRoute] int menteeId)
        {
            var mentorUserId = User.Claims.GetUserId();

            var result = await mentorService.AcceptMenteeAsync(mentorUserId, mentorId, menteeId);

            return ApiResponse(result);
        }

        [HttpPost("{mentorId}/applications/mentees/{menteeId}/reject")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> RejectMentee([FromRoute] int mentorId, [FromRoute] int menteeId)
        {
            var mentorUserId = User.Claims.GetUserId();

            var result = await mentorService.RejectMenteeAsync(mentorUserId, mentorId, menteeId);

            return ApiResponse(result);
        }
    }
}

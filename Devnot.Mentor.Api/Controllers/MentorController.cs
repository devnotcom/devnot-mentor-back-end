using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public MentorController(IMentorService mentorService)
        {
            this.mentorService = mentorService;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetAsync([FromRoute] string userName)
        {
            var result = await mentorService.GetMentorProfile(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/paireds/mentees")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetPairedMentees()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await mentorService.GetPairedMenteesByUserId(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/mentorships")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetMentorships()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await mentorService.GetMentorshipsByUserId(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/applications")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetApplications()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await mentorService.GetApplicationsByUserId(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> Post([FromBody] CreateMentorProfileRequest request)
        {
            request.UserId = User.Claims.GetUserId();

            var result = await mentorService.CreateMentorProfile(request);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost("{mentorId}/applications/mentees/{menteeId}/accept")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> AcceptMentee([FromRoute] int mentorId, [FromRoute] int menteeId)
        {
            var mentorUserId = User.Claims.GetUserId();

            var result = await mentorService.AcceptMentee(mentorUserId, mentorId, menteeId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost("{mentorId}/applications/mentees/{menteeId}/reject")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> RejectMentee([FromRoute] int mentorId, [FromRoute] int menteeId)
        {
            var mentorUserId = User.Claims.GetUserId();

            var result = await mentorService.RejectMentee(mentorUserId, mentorId, menteeId);

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}

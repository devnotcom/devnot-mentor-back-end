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
    public class MentorController : BaseController
    {
        IMentorService mentorService;

        public MentorController(IMentorService service)
        {
            mentorService = service;
        }

        [HttpGet]
        [Route("/mentors/{userName}")]
        public async Task<IActionResult> GetAsync([FromRoute] string userName)
        {
            var result = await mentorService.GetMentorProfile(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("/mentors")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> Post([FromBody] CreateMentorProfileRequest request)
        {
            request.UserId = User.Claims.GetUserId();

            var result = await mentorService.CreateMentorProfile(request);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("/mentors/{mentorId}/mentees/{menteeId}/accept")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> AcceptMentee([FromRoute] int mentorId, [FromRoute] int menteeId)
        {
            var mentorUserId = User.Claims.GetUserId();

            var result = await mentorService.AcceptMentee(mentorUserId, mentorId, menteeId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("/mentors/{mentorId}/mentees/{menteeId}/reject")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> RejectMentee([FromRoute] int mentorId, [FromRoute] int menteeId)
        {
            var mentorUserId = User.Claims.GetUserId();

            var result = await mentorService.RejectMentee(mentorUserId, mentorId, menteeId);

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}

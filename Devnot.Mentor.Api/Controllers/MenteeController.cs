using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Request.MenteeRequest;
using DevnotMentor.Api.Helpers.Extensions;
using Microsoft.AspNetCore.Http;

namespace DevnotMentor.Api.Controllers
{
    [ValidateModelState]
    [ApiController]
    public class MenteeController : BaseController
    {
        private readonly IMenteeService menteeService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public MenteeController(IMenteeService menteeService, IHttpContextAccessor httpContextAccessor)
        {
            this.menteeService = menteeService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("/mentees/{userName}")]
        public async Task<IActionResult> Get([FromRoute] string userName)
        {
            var result = await menteeService.GetMenteeProfile(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("/mentees/{userName}/mentors")]
        public async Task<IActionResult> GetMentors([FromRoute] string userName)
        {
            var result = await menteeService.GetMentors(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }
        
        [HttpGet]
        [Route("/mentees/{userName}/applications")]
        public async Task<IActionResult> GetApplications([FromRoute] string userName)
        {
            var result = await menteeService.GetApplicationsNotIncludeApproveds(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("/mentees")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> Post([FromBody] CreateMenteeProfileRequest request)
        {
            request.UserId = httpContextAccessor.HttpContext.User.Claims.GetUserId();

            var result = await menteeService.CreateMenteeProfile(request);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("/mentees/{menteeId}/mentors")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> ApplyToMentor(ApplyToMentorRequest request)
        {
            request.MenteeUserId = httpContextAccessor.HttpContext.User.Claims.GetUserId();

            var result = await menteeService.ApplyToMentor(request);

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}

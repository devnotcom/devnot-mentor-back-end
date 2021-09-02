using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevnotMentor.Common.Requests;
using DevnotMentor.Common.Requests.Mentee;
using DevnotMentor.Api.Helpers.Extensions;

namespace DevnotMentor.Api.Controllers
{
    [ValidateModelState]
    [ApiController]
    [Route("mentees")]
    public class MenteeController : BaseController
    {
        private readonly IMenteeService menteeService;
        private readonly IMentorshipService MentorshipService;
        private readonly IApplicationService applicationService;

        public MenteeController(IMenteeService menteeService, IMentorshipService MentorshipService, IApplicationService applicationService)
        {
            this.menteeService = menteeService;
            this.MentorshipService = MentorshipService;
            this.applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            var result = await menteeService.SearchAsync(request);

            return ApiResponse(result);
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetMenteeProfileAsync([FromRoute] string userName)
        {
            var result = await menteeService.GetMenteeProfileAsync(userName);

            return ApiResponse(result);
        }
        
        //todo: paireds or mentorship
        [HttpGet("me/paireds/mentors")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetPairedMentorsAsync()
        {
            var authenticatedUserId = User.GetId();
            var result = await menteeService.GetPairedMentorsByUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpGet("me/paireds")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetMentorshipsAsync()
        {
            var authenticatedUserId = User.GetId();
            var result = await MentorshipService.GetMentorshipsOfMenteeByUserId(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> CreateMenteeProfileAsync([FromBody] CreateMenteeProfileRequest request)
        {
            request.UserId = User.GetId();

            var result = await menteeService.CreateMenteeProfileAsync(request);

            return ApiResponse(result);
        }

        [HttpPost("me/applications")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> CreateApplicationAsync([FromBody] ApplicationRequest request)
        {
            request.MenteeUserId = User.GetId();

            var result = await applicationService.CreateApplicationAsync(request);

            return ApiResponse(result);
        }
    }
}

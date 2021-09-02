using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Business.Repository.Interfaces;
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
        private readonly IMenteeService _menteeService;
        private readonly IMentorshipService _mentorshipService;
        private readonly IApplicationService _applicationService;

        public MenteeController(IMenteeService menteeService, IMentorshipService mentorshipService, IApplicationService applicationService)
        {
            _menteeService = menteeService;
            _mentorshipService = mentorshipService;
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            var result = await _menteeService.SearchAsync(request);

            return ApiResponse(result);
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetMenteeProfileAsync([FromRoute] string userName)
        {
            var result = await _menteeService.GetMenteeProfileByUserNameAsync(userName);

            return ApiResponse(result);
        }
        
        [HttpGet("me/paireds/mentors")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetPairedMentorsAsync()
        {
            var authenticatedUserId = User.GetId();
            var result = await _menteeService.GetPairedMentorsByUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpGet("me/mentorships")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetMentorshipsAsync()
        {
            var authenticatedUserId = User.GetId();
            var result = await _mentorshipService.GetMentorshipsByMenteeUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> CreateMenteeProfileAsync([FromBody] CreateMenteeProfileRequest request)
        {
            request.UserId = User.GetId();

            var result = await _menteeService.CreateMenteeProfileAsync(request);

            return ApiResponse(result);
        }

        [HttpPost("me/applications")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> CreateApplicationAsync([FromBody] ApplicationRequest request)
        {
            request.MenteeUserId = User.GetId();

            var result = await _applicationService.CreateApplicationAsync(request);

            return ApiResponse(result);
        }
    }
}

using DevnotMentor.WebAPI.ActionFilters;
using DevnotMentor.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevnotMentor.Common.Requests;
using DevnotMentor.Common.Requests.Mentor;
using DevnotMentor.WebAPI.Helpers.Extensions;

namespace DevnotMentor.WebAPI.Controllers
{
    [ValidateModelState]
    [ApiController]
    [Route("mentors")]
    public class MentorController : BaseController
    {
        private readonly IMentorService _mentorService;
        private readonly IApplicationService _applicationService;
        private readonly IMentorshipService _mentorshipService;

        public MentorController(IMentorService mentorService, IMentorshipService mentorshipService, IApplicationService applicationService)
        {
            _mentorService = mentorService;
            _mentorshipService = mentorshipService;
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            var result = await _mentorService.SearchAsync(request);

            return ApiResponse(result);
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetMentorProfileAsync([FromRoute] string userName)
        {
            var result = await _mentorService.GetMentorProfileByUserNameAsync(userName);

            return ApiResponse(result);
        }
        
        [HttpGet("me/paireds/mentees")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetPairedMenteesAsync()
        {
            var authenticatedUserId = User.GetId();
            var result = await _mentorService.GetPairedMenteesByUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpGet("me/mentorships")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetMentorshipProccesesAsync()
        {
            var authenticatedUserId = User.GetId();
            var result = await _mentorshipService.GetMentorshipsByMentorUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> CreateMentorProfileAsync([FromBody] CreateMentorProfileRequest request)
        {
            request.UserId = User.GetId();

            var result = await _mentorService.CreateMentorProfileAsync(request);

            return ApiResponse(result);
        }

        [HttpPost("me/applications/{id}/approve")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> ApproveApplicationAsync([FromRoute] int id)
        {
            var authenticatedUserId = User.GetId();

            var result = await _applicationService.ApproveWaitingApplicationByIdAsync(authenticatedUserId, id);

            return ApiResponse(result);
        }

        [HttpPost("me/applications/{id}/reject")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> RejectApplicationAsync([FromRoute] int id)
        {
            var authenticatedUserId = User.GetId();

            var result = await _applicationService.RejectWaitingApplicationByIdAsync(authenticatedUserId, id);

            return ApiResponse(result);
        }
    }
}

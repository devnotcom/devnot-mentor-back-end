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
    [Route("mentors")]
    public class MentorController : BaseController
    {
        private readonly IMentorService mentorService;
        private readonly IApplicationService applicationService;
        private readonly IPairService pairService;

        public MentorController(IMentorService mentorService, IPairService pairService, IApplicationService applicationService)
        {
            this.mentorService = mentorService;
            this.pairService = pairService;
            this.applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            var result = await mentorService.SearchAsync(request);

            return ApiResponse(result);
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetMentorProfileAsync([FromRoute] string userName)
        {
            var result = await mentorService.GetMentorProfileAsync(userName);

            return ApiResponse(result);
        }
        
        //todo: paireds or mentorship
        [HttpGet("me/paireds/mentees")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetPairedMenteesAsync()
        {
            var authenticatedUserId = User.GetId();
            var result = await mentorService.GetPairedMenteesByUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpGet("me/paireds")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetMentorshipProccesesAsync()
        {
            var authenticatedUserId = User.GetId();
            var result = await pairService.GetMentorshipsOfMentorByUserIdAsync(authenticatedUserId);

            return ApiResponse(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> CreateMentorProfileAsync([FromBody] CreateMentorProfileRequest request)
        {
            request.UserId = User.GetId();

            var result = await mentorService.CreateMentorProfileAsync(request);

            return ApiResponse(result);
        }

        [HttpPost("me/applications/{id}/approve")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> ApproveApplicationAsync([FromRoute] int id)
        {
            var authenticatedUserId = User.GetId();

            var result = await applicationService.ApproveWaitingApplicationByIdAsync(authenticatedUserId, id);

            return ApiResponse(result);
        }

        [HttpPost("me/applications/{id}/reject")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> RejectApplicationAsync([FromRoute] int id)
        {
            var authenticatedUserId = User.GetId();

            var result = await applicationService.RejectWaitingApplicationByIdAsync(authenticatedUserId, id);

            return ApiResponse(result);
        }
    }
}

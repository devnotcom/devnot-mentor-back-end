using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DevnotMentor.Common.Requests.Mentorship;

namespace DevnotMentor.Api.Controllers
{
    [ServiceFilter(typeof(TokenAuthentication))]
    [ValidateModelState]
    [ApiController]
    public class MentorshipController : BaseController
    {
        private readonly IMentorshipService MentorshipService;

        public MentorshipController(IMentorshipService pairsService)
        {
            this.MentorshipService = pairsService;
        }
        
        //todo: paireds or mentorship
        [HttpPost("/users/me/paireds/{id}/finish")]
        public async Task<IActionResult> FinishAsync([FromRoute] int id)
        {
            var authenticatedUserId = User.GetId();
            var result = await MentorshipService.FinishContinuingPairAsync(authenticatedUserId, id);

            return ApiResponse(result);
        }

        [HttpPost("/users/me/paireds/{id}/feedback")]
        public async Task<IActionResult> FeedbackAsync([FromRoute] int id, [FromBody] MentorshipFeedbackRequest MentorshipFeedbackRequest)
        {
            var authenticatedUserId = User.GetId();
            var result = await MentorshipService.GiveFeedbackToFinishedPairAsync(authenticatedUserId, id, MentorshipFeedbackRequest);

            return ApiResponse(result);
        }
    }
}
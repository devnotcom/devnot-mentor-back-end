using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.CustomEntities.Request.PairRequest;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    [ServiceFilter(typeof(TokenAuthentication))]
    [ValidateModelState]
    [ApiController]
    public class MentorshipController : BaseController
    {
        private readonly IPairService pairService;

        public MentorshipController(IPairService pairsService)
        {
            this.pairService = pairsService;
        }
        
        //todo: paireds or mentorship
        [HttpPost("/users/me/paireds/{id}/finish")]
        public async Task<IActionResult> FinishAsync([FromRoute] int id)
        {
            var authenticatedUserId = User.GetId();
            var result = await pairService.FinishContinuingPairAsync(authenticatedUserId, id);

            return ApiResponse(result);
        }

        [HttpPost("/users/me/paireds/{id}/feedback")]
        public async Task<IActionResult> FeedbackAsync([FromRoute] int id, [FromBody] PairFeedbackRequest pairFeedbackRequest)
        {
            var authenticatedUserId = User.GetId();
            var result = await pairService.GiveFeedbackToFinishedPairAsync(authenticatedUserId, id, pairFeedbackRequest);

            return ApiResponse(result);
        }
    }
}
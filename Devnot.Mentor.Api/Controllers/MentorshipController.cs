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
            var authorizedUserId = User.Claims.GetUserId();
            var result = await pairService.FinishContinuingPairAsync(authorizedUserId, id);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost("/users/me/paireds/{id}/feedback")]
        public async Task<IActionResult> FeedbackAsync([FromRoute] int id, [FromBody] PairFeedbackRequest pairFeedbackRequest)
        {
            var authorizedUserId = User.Claims.GetUserId();
            var result = await pairService.GiveFeedbackToFinishedPairAsync(authorizedUserId, id, pairFeedbackRequest);

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}